using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlowerImageClassification.WebApp.Models
{
	public class Sentiment
	{
		public int ID { get; set; }
		public string FileName { get; set; }
		public string PredictedLabel { get; set; }
		public int Likes { get; set; }
		public int Dislikes { get; set; }
		public int Neutral { get; set; }
		public bool Avaible { get; set; }
		public List<string> MoreInfo { get; set; }
	}
}
