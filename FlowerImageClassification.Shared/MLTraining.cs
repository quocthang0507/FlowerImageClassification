using FlowerImageClassification.Shared.Image;
using Microsoft.ML;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

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

		public MLTraining(string outputModelPath, string inputFolderPathForPrediction, string inputFolderPathForTraining)
		{
			OutputModelPath = outputModelPath;
			InputFolderPathForPrediction = inputFolderPathForPrediction;
			InputFolderPathForTraining = inputFolderPathForTraining;
		}

		/// <summary>
		/// Run prediction method to try single predictions
		/// </summary>
		/// <param name="predictedImagesFolderPath"></param>
		public void RunSinglePrediction(string predictedImagesFolderPath)
		{

		}

		/// <summary>
		/// Run prediction method to try multiple predictions
		/// </summary>
		/// <param name="predictedImagesFolderPath"></param>
		public void RunMultiplePredictions(string predictedImagesFolderPath)
		{
			var predictionEngine = mlContext.Model.CreatePredictionEngine<ImageDataInMemory, ImagePrediction>(trainedModel);
			var testImages = FileUtils.LoadImagesFromDirectoryInMemory(predictedImagesFolderPath, false);

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
