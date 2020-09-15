using Microsoft.ML.Data;

namespace FlowerImageClassification.Shared.Image
{
	public class ImagePrediction
	{
		/// <summary>
		/// Score of this prediction
		/// </summary>
		[ColumnName("Score")]
		public float[] Score;
		/// <summary>
		/// Predicted label of this prediction
		/// </summary>
		[ColumnName("PredictedLabel")]
		public string PredictedLabel;
	}
}
