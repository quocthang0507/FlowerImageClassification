using FlowerImageClassification.Shared;
using FlowerImageClassification.Shared.Common;
using System;
using System.IO;

namespace FlowerImageClassification.Prediction
{
	class Program
	{
		static void Main(string[] args)
		{
			// Redirect console output to a stream
			string output = $"Output_{DateTime.Now.ToString("d M y h m s").Replace(' ', '_')}.txt";
			using (Capturing capturing = new Capturing(output))
			{
				string outputMlNetModelFilePath, imagesFolderPathForPredictions, fullImagesetFolderPath;
				SetPaths(out outputMlNetModelFilePath, out imagesFolderPathForPredictions, out fullImagesetFolderPath);
				MLTraining mlTraining = new MLTraining(outputMlNetModelFilePath, imagesFolderPathForPredictions, fullImagesetFolderPath);
				//mlTraining.TrySinglePrediction();
				mlTraining.TryMultiplePredictions();
			}
			// End the pipeline and write to file
			Console.WriteLine("Press any key to exit...");
			Console.ReadKey();
		}

		static string GetAbsolutePath(string relativePath) => FileUtils.GetAbsolutePath(typeof(Program).Assembly, relativePath);

		static void SetPaths(out string outputMlNetModelFilePath, out string imagesFolderPathForPredictions, out string fullImagesetFolderPath)
		{
			string assetsRelativePath = @"../../../../Assets";
			string assetsPath = GetAbsolutePath(assetsRelativePath);
			outputMlNetModelFilePath = Path.Combine(assetsPath, "Outputs", "imageClassifier.zip");
			imagesFolderPathForPredictions = Path.Combine(assetsPath, "Inputs", "Predictions", "test1");
			fullImagesetFolderPath = Path.Combine(assetsPath, "Inputs", "Trainings", "flower_photos_small_set");
		}

	}
}
