﻿using FlowerImageClassification.Shared;
using FlowerImageClassification.Shared.Common;
using System;
using System.IO;

namespace FlowerImageClassification.Prediction
{
	class Program
	{
		static string outputMlNetModelFilePath, imagesFolderPathForPredictions, fullImagesetFolderPath, consoleOutputPath;
		static string[] trainedModels = { "imageClassifier_inceptionv3.zip", "imageClassifier_mobilenetv2.zip", "imageClassifier_resnetv250.zip", "imageClassifier_resnetv2101.zip" };

		static void Main(string[] args)
		{
			// Redirect console output to a stream
			SetPaths();
			foreach (var model in trainedModels)
			{
				string output = Path.Combine(consoleOutputPath, $"EvaluatingOutput_{model}_{DateTime.Now.ToString("d_M_y h_m_s")}.txt");
				outputMlNetModelFilePath = Path.Combine(outputMlNetModelFilePath, model);
				using (MirrorOutput capturing = new MirrorOutput(output))
				{
					MLTraining mlTraining = new MLTraining(outputMlNetModelFilePath, imagesFolderPathForPredictions, fullImagesetFolderPath);
					mlTraining.EvaluateModel();
					//mlTraining.TrySinglePrediction();
					//mlTraining.TryMultiplePredictions();
				}
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
			outputMlNetModelFilePath = Path.Combine(assetsPath, "Outputs");
			imagesFolderPathForPredictions = Path.Combine(assetsPath, "Inputs", "Predictions", "test1");
			fullImagesetFolderPath = Path.Combine(assetsPath, "Inputs", "Trainings", "dalat_dataset");
			consoleOutputPath = GetAbsolutePath(@"../../../../Outputs");
		}

	}
}
