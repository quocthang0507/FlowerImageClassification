using System;

namespace FlowerImageClassification.WebApp.Models
{
	public class Sentiment
	{
		public int ID { get; set; }
		public string FileName { get; set; }
		public string PredictedLabel { get; set; }
		public string NewLabel { get; set; }
		public DateTime UploadDate { get; set; }
		public bool Visible { get; set; }

		public Sentiment(string fileName, string predictedLabel, DateTime uploadDate)
		{
			FileName = fileName;
			PredictedLabel = predictedLabel;
			UploadDate = uploadDate;
			Visible = true;
		}

	}
}
