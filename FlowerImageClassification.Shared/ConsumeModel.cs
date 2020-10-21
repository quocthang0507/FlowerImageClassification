using FlowerImageClassification.Shared.ImageSchema;
using Microsoft.ML;
using System;

namespace FlowerImageClassification.Shared
{
	public class ConsumeModel
	{
		private static Lazy<PredictionEngine<ImageDataInMemory, ImagePrediction>> PredictionEngine = new Lazy<PredictionEngine<ImageDataInMemory, ImagePrediction>>(CreatePredictionEngine);

		public static string MLNetModelPath { get; set; }

		// For more info on consuming ML.NET models, visit https://aka.ms/mlnet-consume
		// Method for consuming model in your app
		public static ImagePrediction Predict(ImageDataInMemory input)
		{
			ImagePrediction result = PredictionEngine.Value.Predict(input);
			return result;
		}

		public static PredictionEngine<ImageDataInMemory, ImagePrediction> CreatePredictionEngine()
		{
			// Create new MLContext
			MLContext mlContext = new MLContext();
			// Load model & create prediction engine
			try
			{
				var mlModel = mlContext.Model.Load(MLNetModelPath, out var modelInputSchema);
				var predEngine = mlContext.Model.CreatePredictionEngine<ImageDataInMemory, ImagePrediction>(mlModel);
				return predEngine;
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
				throw;
			}
		}
	}
}
