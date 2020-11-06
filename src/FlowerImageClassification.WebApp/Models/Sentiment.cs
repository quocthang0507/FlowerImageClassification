namespace FlowerImageClassification.WebApp.Models
{
	public class Sentiment
	{
		public int ID { get; set; }
		public string FileName { get; set; }
		public string PredictedLabel { get; set; }
		public uint Likes { get; set; }
		public uint Dislikes { get; set; }
		public uint Neutral { get; set; }
		public bool Avaiable { get; set; }
		public uint IncorrectPredictionVotes { get; set; }
		public uint UnusefulInfoVotes { get; set; }
		public uint DelayResponseVotes { get; set; }
		public uint HardToUseVotes { get; set; }
		public string MoreInfo { get; set; }
	}
}
