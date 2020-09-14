using FlowerImageClassification.Shared;
using System;
using System.IO;

namespace FlowerImageClassification.Training
{
	class Program
	{
		static void Main(string[] args)
		{
			// Redirect console output to a stream
			StreamWriter writer = new StreamWriter($"Output_{DateTime.Now.ToString("d M y h m s").Replace(' ', '_')}.txt");
			TextWriter consoleOut = Console.Out;
			Console.SetOut(writer);
			// Begin to run the pipeline
			string outputMlNetModelFilePath, imagesFolderPathForPredictions, fullImagesetFolderPath;
			SetPaths(out outputMlNetModelFilePath, out imagesFolderPathForPredictions, out fullImagesetFolderPath);
			MLTraining mlTraining = new MLTraining(outputMlNetModelFilePath, imagesFolderPathForPredictions, fullImagesetFolderPath);
			mlTraining.RunPipeline();
			// End the pipeline and write to file
			Console.SetOut(consoleOut);
			writer.Close();

			Console.ReadKey();
		}

		static string GetAbsolutePath(string relativePath) => FileUtils.GetAbsolutePath(typeof(Program).Assembly, relativePath);

		static void SetPaths(out string outputMlNetModelFilePath, out string imagesFolderPathForPredictions, out string fullImagesetFolderPath)
		{
			string assetsRelativePath = @"../../../../assets";
			string assetsPath = GetAbsolutePath(assetsRelativePath);
			outputMlNetModelFilePath = Path.Combine(assetsPath, "Outputs");
			imagesFolderPathForPredictions = Path.Combine(assetsPath, "Inputs", "Predictions", "test1");
			fullImagesetFolderPath = Path.Combine(assetsPath, "Inputs", "Trainings", "flower_photos_small_set");
		}
	}
}
