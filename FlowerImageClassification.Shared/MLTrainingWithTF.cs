using FlowerImageClassification.Shared.Image;
using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.ML.Transforms;

namespace FlowerImageClassification.Shared
{
	public class MLTrainingWithTF
	{
		/// <summary>
		/// Path to inception model .pb file
		/// </summary>
		public string InceptionTFModelPath { get; set; }

		private MLContext mlContext;

		public MLTrainingWithTF(string inceptionTFModelPath)
		{
			InceptionTFModelPath = inceptionTFModelPath;
			mlContext = new MLContext();
		}

		public EstimatorChain<KeyToValueMappingTransformer> CreatePipeline()
		{
			IEstimator<ITransformer> pipeline = mlContext.Transforms.
				LoadImages(outputColumnName: "input", imageFolder: _imagesFolder, inputColumnName: nameof(ImageData.ImagePath)).
				Append(mlContext.Transforms.ResizeImages(outputColumnName: "input", imageWidth: InceptionSettings.ImageWidth, imageHeight: InceptionSettings.ImageHeight, inputColumnName: "input")).
				Append(mlContext.Transforms.ExtractPixels(outputColumnName: "input", interleavePixelColors: InceptionSettings.ChannelsLast, offsetImage: InceptionSettings.Mean)).
				Append(mlContext.Model.LoadTensorFlowModel(InceptionTFModelPath).
				ScoreTensorFlowModel(outputColumnNames: new[] { "softmax2_pre_activation" }, inputColumnNames: new[] { "input" }, addBatchDimensionInput: true)).
				Append(mlContext.Transforms.Conversion.MapValueToKey(outputColumnName: "LabelKey", inputColumnName: "Label")).
				Append(mlContext.MulticlassClassification.Trainers.LbfgsMaximumEntropy(labelColumnName: "LabelKey", featureColumnName: "softmax2_pre_activation")).
				Append(mlContext.Transforms.Conversion.MapKeyToValue("PredictedLabelValue", "PredictedLabel")).AppendCacheCheckpoint(mlContext);
			return pipeline;
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
