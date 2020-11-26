namespace FlowerImageClassification.Shared.ImageSchema
{
	/// <summary>
	/// Image result with predicted label and its probability
	/// </summary>
	public class ImagePredictedLabelWithProbability
	{
		public string ImageID { get; set; }
		public string PredictedLabel { get; set; }
		public float Probability { get; set; }
		public long PredictionExecutionTime { get; set; }
		public string VietnameseLabel { get; set; }
	}
}
