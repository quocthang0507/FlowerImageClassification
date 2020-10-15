using FlowerImageClassification.Shared;
using FlowerImageClassification.Shared.Common;
using System;
using System.IO;

namespace FlowerImageClassification.Training
{
	public enum Architecture
	{
		ResnetV2101 = 0,
		InceptionV3 = 1,
		MobilenetV2 = 2,
		ResnetV250 = 3
	}

	class Program
	{
		static string outputMlNetModelFilePath, imagesFolderPathForPredictions, fullImagesetFolderPath, consoleOutputPath, inceptionModelPath;
		static string architectureName;

		static void Main(string[] args)
		{
			int arch = 0;
			architectureName = Enum.GetName(typeof(Architecture), arch);

			// Redirect console output to a stream
			SetPaths();
			string output = Path.Combine(consoleOutputPath, $"{architectureName}_{DateTime.Now.ToString("d_M_y h_m_s")}.txt");
			using (MirrorOutput capturing = new MirrorOutput(output))
			{
				// Begin to run the pipeline
				MLTraining mlTraining = new MLTraining(outputMlNetModelFilePath, imagesFolderPathForPredictions, fullImagesetFolderPath, null, 0.5f, arch);
				mlTraining.RunPipeline();
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
			outputMlNetModelFilePath = Path.Combine(assetsPath, "Outputs", $"imageClassifier_{architectureName}.zip");
			imagesFolderPathForPredictions = Path.Combine(assetsPath, "Inputs", "Predictions", "test1");
			fullImagesetFolderPath = Path.Combine(assetsPath, "Inputs", "Trainings", "dalat_dataset");
			consoleOutputPath = GetAbsolutePath(@"../../../../Outputs");
			inceptionModelPath = Path.Combine(assetsPath, "Inputs", "Inception", "tensorflow_inception_graph.pb");
		}
	}
}
