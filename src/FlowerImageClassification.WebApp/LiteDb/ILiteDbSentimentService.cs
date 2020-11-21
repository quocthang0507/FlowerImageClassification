using FlowerImageClassification.WebApp.Models;
using System.Collections.Generic;

namespace FlowerImageClassification.WebApp.LiteDb
{
	public interface ILiteDbSentimentService
	{
		IEnumerable<Sentiment> FindAll();
		Sentiment FindOne(int id);
		int Insert(Sentiment sentiment);
		bool Update(Sentiment sentiment);
		bool Delete(int id);
	}
}
