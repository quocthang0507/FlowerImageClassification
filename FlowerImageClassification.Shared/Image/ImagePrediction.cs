using Microsoft.ML.Data;
using System;
using System.Collections.Generic;
using System.Text;

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
