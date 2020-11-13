using FlowerImageClassification.Shared;
using FlowerImageClassification.Shared.Common;
using FlowerImageClassification.Shared.WebHelpers;
using System;
using System.IO;

namespace FlowerImageClassification.Training
{
	class Program
	{
		static string outputMlNetModelFilePath, imagesFolderPathForPredictions, fullImagesetFolderPath, consoleOutputPath, inceptionModelPath;
		static string architectureName;
		static float[] fractions = { 0.5f, 0.6f, 0.7f, 0.8f, 0.9f };
		static string fileName;

		static void Main(string[] args)
		{

			//foreach (Architecture arch in (Architecture[])Enum.GetValues(typeof(Architecture)))
			//{
			//	foreach (float frac in fractions)
			//	{
			//		architectureName = Enum.GetName(typeof(Architecture), (int)arch);
			//		fileName = $"{architectureName}_{frac}";

			//		SetPaths();
			//		string output = Path.Combine(consoleOutputPath, fileName + ".txt");

			//		OutputHelper capturing = new OutputHelper(output);
			//		Console.WriteLine($"==================== { architectureName} architecture, {frac} ratio of train set with test set ====================");
			//		MLTraining mlTraining = new MLTraining(outputMlNetModelFilePath, imagesFolderPathForPredictions, fullImagesetFolderPath, null, frac, (int)arch);
			//		mlTraining.RunPipeline();
			//		capturing.Dispose();
			//	}
			//}

			// Redirect console output to a stream
			// SetPaths();
			Console.WriteLine($"========== {architectureName} architecture ==========");
			MLTraining mlTraining = new MLTraining(
				@"D:\Github\FlowerImageClassification\src\FlowerImageClassification.WebApp\ML\InceptionV3.zip",
				@"D:\Github\FlowerImageClassification\src\Assets\Imagesets\Trainings\kaggle_dataset_5_1\Training 0.8",
				@"D:\Github\FlowerImageClassification\src\Assets\Imagesets\Trainings\kaggle_dataset_5_1\Evaluation 0.2",
				1,
				1,
				(int)Architecture.MobilenetV2,
				true);
			// Begin to run the pipeline
			mlTraining.RunPipeline();

			// End the pipeline and write to file
			Console.WriteLine("Press any key to exit...");
			Console.ReadKey();
		}

		static string GetAbsolutePath(string relativePath) => FileUtils.GetAbsolutePath(typeof(Program).Assembly, relativePath);

		static void SetPaths()
		{
			string assetsRelativePath = @"../../../../Assets";
			string assetsPath = GetAbsolutePath(assetsRelativePath);
			outputMlNetModelFilePath = Path.Combine(assetsPath, "Outputs", "Lan 2", fileName + ".zip");
			imagesFolderPathForPredictions = Path.Combine(assetsPath, "Inputs", "Predictions", "test1");
			fullImagesetFolderPath = Path.Combine(assetsPath, "Inputs", "Trainings", "dalat_dataset");
			consoleOutputPath = GetAbsolutePath(@"../../../../ConsoleOutputs/Lan 2 (Thu nho Dataset)");
			inceptionModelPath = Path.Combine(assetsPath, "Inputs", "Inception", "tensorflow_inception_graph.pb");
		}
	}
}
