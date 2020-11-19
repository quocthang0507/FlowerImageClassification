namespace FlowerImageClassification.WebApp.Models
{
	public class Sentiment
	{
		public int ID { get; set; }
		public string FileName { get; set; }
		public string PredictedLabel { get; set; }
		public int LikeNumber { get; set; }
		public int DislikeNumber { get; set; }

		public Sentiment(string fileName, string predictedLabel, int likeNumber, int dislikeNumber)
		{
			FileName = fileName;
			PredictedLabel = predictedLabel;
			LikeNumber = likeNumber;
			DislikeNumber = dislikeNumber;
		}

	}
}
