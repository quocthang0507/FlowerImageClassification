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
			string model = "imageClassifier_resnetv2101.zip"; // Must put the name of trained model file
			string output = Path.Combine(consoleOutputPath, $"EvaluatingOutput_{model}_{DateTime.Now.ToString("d_M_y h_m_s")}.txt");
			outputMlNetModelFilePath = Path.Combine(outputMlNetModelFilePath, model);
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
			outputMlNetModelFilePath = Path.Combine(assetsPath, "Outputs", "Old");
			imagesFolderPathForPredictions = Path.Combine(assetsPath, "Inputs", "Predictions");
			fullImagesetFolderPath = Path.Combine(assetsPath, "Inputs", "Trainings", "dalat_dataset");
			consoleOutputPath = GetAbsolutePath(@"../../../../Outputs");
		}

	}
}
