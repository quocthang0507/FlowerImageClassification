﻿using FlowerImageClassification.Shared.Common;
using FlowerImageClassification.Shared.Image;
using Microsoft.ML;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using static Microsoft.ML.Transforms.ValueToKeyMappingEstimator;

namespace FlowerImageClassification.Shared
{
	/// <summary>
	/// Class provides many methods to train a model using ML.Net
	/// </summary>
	public class MLTraining
	{
		/// <summary>
		/// Path to output folder where ML model saved
		/// </summary>
		public string OutputModelPath { get; set; }
		/// <summary>
		/// Path to image folder for prediction
		/// </summary>
		public string InputFolderPathForPrediction { get; set; }
		/// <summary>
		/// Path to image folder for training
		/// </summary>
		public string InputFolderPathForTraining { get; set; }

		private MLContext mlContext;
		private ITransformer trainedModel;
		private IDataView testDataset;

		public MLTraining(string outputModelPath, string inputFolderPathForPrediction, string inputFolderPathForTraining)
		{
			OutputModelPath = outputModelPath;
			InputFolderPathForPrediction = inputFolderPathForPrediction;
			InputFolderPathForTraining = inputFolderPathForTraining;
			// MLContext's random number generator is the global source of randomness for all of such random operations.
			mlContext = new MLContext(1);
			mlContext.Log += PrintMLContextLog;
		}

		/// <summary>
		/// Run the pipeline to train the model
		/// </summary>
		public void TrainModel()
		{
			// 2. Load the initial full image-set into an IDataView and shuffle so it'll be better balanced
			IEnumerable<ImageData> images = FileUtils.LoadImagesFromDirectory(InputFolderPathForTraining, true);
			IDataView dataset = mlContext.Data.LoadFromEnumerable(images);
			IDataView shuffledDataset = mlContext.Data.ShuffleRows(dataset);

			// 3. Load Images with in-memory type within the IDataView and Transform Labels to Keys (Categorical)
			IDataView transformedDataset = mlContext.Transforms.Conversion.
				MapValueToKey("LabelAsKey", "Label", keyOrdinality: KeyOrdinality.ByValue)
				.Append(mlContext.Transforms.LoadRawImageBytes("Image", InputFolderPathForTraining, "ImagePath")).
				Fit(shuffledDataset).
				Transform(shuffledDataset);

			// 4. Split the data 80:20 into train and test sets, train and evaluate.
			var splitData = mlContext.Data.TrainTestSplit(transformedDataset, 0.2);
			IDataView trainDataset = splitData.TrainSet; // 80%
			IDataView testDataset = splitData.TestSet; // 20%
		}

		/// <summary>
		/// Evaluate model by making predictions in bulk
		/// </summary>
		public void EvaluateModel()
		{
			Console.WriteLine("Making predictions in bulk for evaluating model's quality...");
			var watch = Stopwatch.StartNew();
			var predictionsDataView = trainedModel.Transform(testDataset);
			var metrics = mlContext.MulticlassClassification.Evaluate(predictionsDataView, "LabelAsKey", "PredictedLabel");
			ConsoleHelper.PrintMultiClassClassificationMetrics("TensorFlow DNN Transfer Learning", metrics);
			watch.Stop();
			var milliseconds = watch.ElapsedMilliseconds;
			Console.WriteLine($"Predicting and Evaluation took: {milliseconds / 1000} seconds");
		}

		/// <summary>
		/// Get the first image file and run prediction method
		/// </summary>
		/// <param name="predictedImagesFolderPath"></param>
		public void RunSinglePrediction(string predictedImagesFolderPath)
		{
			var predictionEngine = mlContext.Model.CreatePredictionEngine<ImageDataInMemory, ImagePrediction>(trainedModel);
			var predictedImages = FileUtils.LoadImagesFromDirectoryInMemory(predictedImagesFolderPath, false);
			var image = predictedImages.First();
			var prediction = predictionEngine.Predict(image);
			PrintImagePrediction(image.ImageData.ImagePath, "Unknown", prediction.PredictedLabel, prediction.Score.Max());
		}

		/// <summary>
		/// Run prediction method to try multiple predictions
		/// </summary>
		/// <param name="predictedImagesFolderPath"></param>
		public void RunMultiplePredictions(string predictedImagesFolderPath)
		{
			var predictionEngine = mlContext.Model.CreatePredictionEngine<ImageDataInMemory, ImagePrediction>(trainedModel);
			var predictedImages = FileUtils.LoadImagesFromDirectoryInMemory(predictedImagesFolderPath, false);
			foreach (var image in predictedImages)
			{
				var prediction = predictionEngine.Predict(image);
				PrintImagePrediction(image.ImageData.ImagePath, "Unknown", prediction.PredictedLabel, prediction.Score.Max());
			}
		}

		/// <summary>
		/// Console.Write image prediction's result
		/// </summary>
		/// <param name="imagePath"></param>
		/// <param name="label"></param>
		/// <param name="predictedLabel"></param>
		/// <param name="probaility"></param>
		private void PrintImagePrediction(string imagePath, string label, string predictedLabel, float probability)
		{
			var defaultForeground = Console.ForegroundColor;
			var labelColor = ConsoleColor.Magenta;
			var probColor = ConsoleColor.Blue;
			Console.Write("Image File: ");
			Console.ForegroundColor = labelColor;
			Console.Write($"{Path.GetFileName(imagePath)}");
			Console.ForegroundColor = defaultForeground;
			Console.Write(" original labeled as ");
			Console.ForegroundColor = labelColor;
			Console.Write(label);
			Console.ForegroundColor = defaultForeground;
			Console.Write(" predicted as ");
			Console.ForegroundColor = labelColor;
			Console.Write(predictedLabel);
			Console.ForegroundColor = defaultForeground;
			Console.Write(" with score ");
			Console.ForegroundColor = probColor;
			Console.Write(probability);
			Console.ForegroundColor = defaultForeground;
			Console.WriteLine("");
		}

		/// <summary>
		/// Console.Write filtered MLContext logs
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void PrintMLContextLog(object sender, LoggingEventArgs e)
		{
			if (e.Message.StartsWith("[Source=ImageClassificationTrainer;"))
			{
				Console.WriteLine(e.Message);
			}
		}
	}
}
