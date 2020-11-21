namespace FlowerImageClassification.WebApp.Models
{
	public class Sentiment
	{
		public int ID { get; set; }
		public string FileName { get; set; }
		public string PredictedLabel { get; set; }
		public string NewLabel { get; set; }

		public Sentiment(string fileName, string predictedLabel, string newLabel)
		{
			FileName = fileName;
			PredictedLabel = predictedLabel;
			NewLabel = newLabel;
		}

	}
}
