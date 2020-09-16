using FlowerImageClassification.Shared;
using FlowerImageClassification.Shared.Common;
using System;
using System.IO;

namespace FlowerImageClassification.Prediction
{
	class Program
	{
		static string outputMlNetModelFilePath, imagesFolderPathForPredictions, fullImagesetFolderPath, consoleOutputPath;

		static void Main(string[] args)
		{
			// Redirect console output to a stream
			SetPaths();
			string output = Path.Combine(consoleOutputPath, $"EvaluatingOutput_{DateTime.Now.ToString("d_M_y h_m_s")}.txt");
			using (MirrorOutput capturing = new MirrorOutput(output))
			{
				MLTraining mlTraining = new MLTraining(outputMlNetModelFilePath, imagesFolderPathForPredictions, fullImagesetFolderPath);
				//mlTraining.EvaluateModel();
				//mlTraining.TrySinglePrediction();
				mlTraining.TryMultiplePredictions();
			}
			// End the pipeline and write to file
			Console.WriteLine("Press any key to exit...");
			Console.ReadKey();
		}

		static string GetAbsolutePath(string relativePath) => FileUtils.GetAbsolutePath(typeof(Program).Assembly, relativePath);

		static void SetPaths()
		{
			string assetsRelativePath = @"../../../../Assets";
			string assetsPath = GetAbsolutePath(assetsRelativePath);
			outputMlNetModelFilePath = Path.Combine(assetsPath, "Outputs", "imageClassifier.zip");
			imagesFolderPathForPredictions = Path.Combine(assetsPath, "Inputs", "Predictions", "test1");
			fullImagesetFolderPath = Path.Combine(assetsPath, "Inputs", "Trainings", "flower_photos_small_set");
			consoleOutputPath = GetAbsolutePath(@"../../../../Outputs");
		}

	}
}
