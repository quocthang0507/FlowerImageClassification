﻿using FlowerImageClassification.Shared.Common;
using FlowerImageClassification.Shared.Image;
using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.ML.Transforms;
using Microsoft.ML.Vision;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
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
		private IDataView trainDataset;

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
		/// Run the pipeline to train the model, then save the model to specific output folder path
		/// </summary>
		public void RunPipeline()
		{
			// 1., 2., 3., 4.
			PrepareDataset();

			// 5. Call pipeline
			//var pipeline = CreateDefaultPipeline(testDataset);
			var pipeline = CreateCustomPipeline(testDataset);

			// 6. Train/create the ML Model
			Console.WriteLine("*** Training the image classification model with DNN Transfer Learning on top of the selected pre-trained model/architecture ***");

			////////// Begin training
			var watch = Stopwatch.StartNew();
			trainedModel = pipeline.Fit(trainDataset);
			watch.Stop();
			////////// End training

			var ms = watch.ElapsedMilliseconds;
			Console.WriteLine($"Training with transfer learning took: {ms / 1000} seconds");

			// 7. Get the quality metrics
			EvaluateModel();

			// 8. Save the model to assets/outputs ML.NET .zip model file and TF .pb model file
			mlContext.Model.Save(trainedModel, trainDataset.Schema, OutputModelPath);
			Console.WriteLine($"Model saved to: {OutputModelPath}");
		}

		/// <summary>
		/// Evaluate model by making predictions in bulk.
		/// If you run it without running pipeline, it will find and load the existed trained model, and then prepare the dataset.
		/// Maybe the evaluation result different in each running.
		/// </summary>
		public void EvaluateModel()
		{
			if (trainedModel == null)
			{
				if (File.Exists(OutputModelPath))
				{
					LoadTrainedModel();
					PrepareDataset();
				}
				else
					throw new Exception("Please run the pipeline before evaluating!");
			}
			Console.WriteLine("Making predictions in bulk for evaluating model's quality...");
			// Begin evaluating
			var watch = Stopwatch.StartNew();
			var predictionsDataView = trainedModel.Transform(testDataset);
			var metrics = mlContext.MulticlassClassification.Evaluate(predictionsDataView, labelColumnName: "LabelAsKey", predictedLabelColumnName: "PredictedLabel");
			ConsoleHelper.PrintMultiClassClassificationMetrics("TensorFlow DNN Transfer Learning", metrics);
			watch.Stop();
			// End evaluating
			var milliseconds = watch.ElapsedMilliseconds;
			Console.WriteLine($"Predicting and Evaluation took: {milliseconds / 1000} seconds");
		}

		/// <summary>
		/// Get the first image file and run prediction method
		/// </summary>
		public void TrySinglePrediction()
		{
			if (trainedModel == null)
			{
				if (File.Exists(OutputModelPath))
					LoadTrainedModel();
				else
					throw new Exception("Please run the pipeline before predicting!");
			}
			var predictionEngine = mlContext.Model.CreatePredictionEngine<ImageDataInMemory, ImagePrediction>(trainedModel);
			var predictedImages = FileUtils.LoadImagesFromDirectoryInMemory(InputFolderPathForPrediction, false);
			var image = predictedImages.First();
			var prediction = predictionEngine.Predict(image);
			PrintImagePrediction(image.ImagePath, image.Label, prediction.PredictedLabel, prediction.Score.Max());
		}

		/// <summary>
		/// Run prediction method to try multiple predictions
		/// </summary>
		public void TryMultiplePredictions()
		{
			if (trainedModel == null)
			{
				if (File.Exists(OutputModelPath))
					LoadTrainedModel();
				else
					throw new Exception("Please run the pipeline before predicting!");
			}
			var predictionEngine = mlContext.Model.CreatePredictionEngine<ImageDataInMemory, ImagePrediction>(trainedModel);
			var predictedImages = FileUtils.LoadImagesFromDirectoryInMemory(InputFolderPathForPrediction, false);
			foreach (var image in predictedImages)
			{
				var prediction = predictionEngine.Predict(image);
				PrintImagePrediction(image.ImagePath, image.Label, prediction.PredictedLabel, prediction.Score.Max());
			}
		}

		/// <summary>
		/// Prepare dataset by loading from files, transforming and splitting
		/// </summary>
		private void PrepareDataset()
		{
			// 2. Load the initial full image-set into an IDataView and shuffle so it'll be better balanced
			IEnumerable<ImageData> images = FileUtils.LoadImagesFromDirectory(InputFolderPathForTraining, true);
			IDataView dataset = mlContext.Data.LoadFromEnumerable(images);
			IDataView shuffledDataset = mlContext.Data.ShuffleRows(dataset);

			// 3. Load Images with in-memory type within the IDataView and Transform Labels to Keys (Categorical)
			IDataView transformedDataset = mlContext.Transforms.Conversion.
				MapValueToKey("LabelAsKey", "Label", keyOrdinality: KeyOrdinality.ByValue)
				// The outputColumnName should has same name in ImageDataInMemory
				.Append(mlContext.Transforms.LoadRawImageBytes("ImageBytes", InputFolderPathForTraining, "ImagePath")).
				Fit(shuffledDataset).
				Transform(shuffledDataset);

			// 4. Split the data 80:20 into train and test sets, train and evaluate.
			var splitData = mlContext.Data.TrainTestSplit(transformedDataset, 0.2);
			trainDataset = splitData.TrainSet; // 80%
			testDataset = splitData.TestSet; // 20%
		}

		/// <summary>
		/// Load trained model from .zip file
		/// </summary>
		private void LoadTrainedModel()
		{
			Console.WriteLine($"Loading model from: {OutputModelPath}");
			trainedModel = mlContext.Model.Load(OutputModelPath, out var modelSchema);
		}

		/// <summary>
		/// 5. Define the model's training pipeline using DNN default values
		/// </summary>
		/// <param name="dataset"></param>
		/// <returns></returns>
		private EstimatorChain<KeyToValueMappingTransformer> CreateDefaultPipeline(IDataView dataset)
		{
			var pipeline = mlContext.MulticlassClassification.Trainers.
				// The feature column name should has same name in ImageDataInMemory
				ImageClassification(labelColumnName: "LabelAsKey", featureColumnName: "ImageBytes", validationSet: dataset).
				Append(mlContext.Transforms.Conversion.MapKeyToValue("PredictedLabel", "PredictedLabel"));
			return pipeline;
		}

		/// <summary>
		/// 5.1. (Optional) Define the model's training pipeline by using explicit hyper-parameters
		/// </summary>
		/// <param name="dataset"></param>
		/// <returns></returns>
		private EstimatorChain<KeyToValueMappingTransformer> CreateCustomPipeline(IDataView dataset)
		{
			var options = new ImageClassificationTrainer.Options()
			{
				LabelColumnName = "LabelAsKey",
				// The feature column name should has same name in ImageDataInMemory
				FeatureColumnName = "ImageBytes",
				// Change the architecture to different DNN architecture
				Arch = ImageClassificationTrainer.Architecture.ResnetV250,
				// Number of training iterations
				Epoch = 100,
				// Number of samples to use for mini-batch training
				BatchSize = 10,
				LearningRate = 0.01f,
				MetricsCallback = (metrics) => Console.WriteLine(metrics),
				ValidationSet = dataset
			};
			var pipeline = mlContext.MulticlassClassification.Trainers.ImageClassification(options).
				Append(mlContext.Transforms.Conversion.MapKeyToValue("PredictedLabel", "PredictedLabel"));
			return pipeline;
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
