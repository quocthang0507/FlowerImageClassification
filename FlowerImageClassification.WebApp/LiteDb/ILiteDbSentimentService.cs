using FlowerImageClassification.WebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlowerImageClassification.WebApp.LiteDb
{
	public interface ILiteDbSentimentService
	{
		IEnumerable<Sentiment> FindAll();
		Sentiment FindOne(int id);
		int Insert(Sentiment sentiment);
		bool Update(Sentiment sentiment);

	}
}
