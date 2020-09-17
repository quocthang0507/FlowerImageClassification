namespace FlowerImageClassification.Shared.Image
{
	/// <summary>
	/// Image result with predicted label and its probability
	/// </summary>
	public class ImagePredictedLabelWithProbability
	{
		public string ImageID;
		public string PredictedLabel;
		public float Probability { get; set; }
		public long PredictionExecutionTime;
	}
}
