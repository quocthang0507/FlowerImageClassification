using FlowerImageClassification.WebApp.Models;
using LiteDB;
using System.Collections.Generic;
using System.Linq;

namespace FlowerImageClassification.WebApp.LiteDb
{
	public class LiteDbSentimentService : ILiteDbSentimentService
	{
		private LiteDatabase liteDb;

		public LiteDbSentimentService(ILiteDbContext liteDbContext)
		{
			liteDb = liteDbContext.Database;
		}

		public IEnumerable<Sentiment> FindAll() => liteDb.GetCollection<Sentiment>().FindAll();

		public Sentiment FindOne(int id) => liteDb.GetCollection<Sentiment>().Find(s => s.ID == id).FirstOrDefault();

		public int Insert(Sentiment sentiment) => liteDb.GetCollection<Sentiment>().Insert(sentiment);

		public bool Update(Sentiment sentiment) => liteDb.GetCollection<Sentiment>().Update(sentiment);
	}
}
