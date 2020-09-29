using Microsoft.ML;
using Microsoft.ML.Vision;
using System;
using System.Diagnostics;
using static Microsoft.ML.Transforms.ValueToKeyMappingEstimator;

namespace FlowerImageClassification.Shared
{
	/// <summary>
	/// Class provides many methods to train a model using ML.Net Transfer Learning
	/// </summary>
	public class MLTrainingWithTF : MLTraining
	{
		/// <summary>
		/// Path to inception model .pb file
		/// </summary>
		public string InceptionTFModelPath { get; set; }

		public MLTrainingWithTF(string inceptionTFModelPath, string outputModelPath, string inputFolderPathForPrediction, string inputFolderPathForTraining) :
			base(outputModelPath, inputFolderPathForPrediction, inputFolderPathForTraining)
		{
			InceptionTFModelPath = inceptionTFModelPath;
		}

		public IEstimator<ITransformer> CreateCustomPipelineTF()
		{
			var options = new ImageClassificationTrainer.Options()
			{
				LabelColumnName = "LabelAsKey",
				FeatureColumnName = "softmax2_pre_activation",
				// Change the architecture to different DNN architecture
				Arch = ImageClassificationTrainer.Architecture.InceptionV3,
				// Number of training iterations
				Epoch = 200,
				// Number of samples to use for mini-batch training
				BatchSize = 10,
				LearningRate = 0.01f,
				MetricsCallback = (metrics) => Console.WriteLine(metrics),
				ValidationSet = validationDataset
			};
			string outputCol = "ImageBytes", inputCol = "ImagePath";
			IEstimator<ITransformer> pipeline = mlContext.Transforms.
				LoadRawImageBytes(outputCol, InputFolderPathForTraining, inputCol).
				Append(mlContext.Transforms.ResizeImages(outputCol, imageWidth: InceptionSettings.ImageWidth, imageHeight: InceptionSettings.ImageHeight, inputCol)).
				Append(mlContext.Transforms.ExtractPixels(outputCol, interleavePixelColors: InceptionSettings.ChannelsLast, offsetImage: InceptionSettings.Mean)).
				Append(mlContext.Model.LoadTensorFlowModel(InceptionTFModelPath).
				ScoreTensorFlowModel(outputColumnNames: new[] { "softmax2_pre_activation" }, inputColumnNames: new[] { inputCol }, addBatchDimensionInput: true)).
				Append(mlContext.Transforms.Conversion.MapValueToKey("LabelAsKey", "Label", keyOrdinality: KeyOrdinality.ByValue)).
				////////////////////////////////////////////////////////////////////////////////
				Append(mlContext.MulticlassClassification.Trainers.ImageClassification(options)).
				//Append(mlContext.MulticlassClassification.Trainers.LbfgsMaximumEntropy(labelColumnName: "LabelAsKey", featureColumnName: "softmax2_pre_activation")).
				Append(mlContext.Transforms.Conversion.MapKeyToValue("PredictedLabel", "Predicted")).
				AppendCacheCheckpoint(mlContext);
			return pipeline;
		}

		/// <summary>
		/// Run the pipeline to train the model, then save the model to specific output folder path
		/// </summary>
		public new void RunPipeline()
		{
			// 1., 2., 3., 4.
			PrepareDataset();

			// 5. Call pipeline
			var pipeline = CreateCustomPipelineTF();

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
		/// Map the parameter values to friendly names
		/// </summary>
		private struct InceptionSettings
		{
			public const int ImageHeight = 224;
			public const int ImageWidth = 224;
			public const float Mean = 117;
			public const float Scale = 1;
			public const bool ChannelsLast = true;
		}
	}
}
