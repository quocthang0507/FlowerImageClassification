using FlowerImageClassification.WebApp.Models;
using System.Collections.Generic;

namespace FlowerImageClassification.WebApp.LiteDb
{
	public interface ILiteDbFlowerService
	{
		int Delete(int id);
		IEnumerable<Flower> FindAll();
		Flower FindOne(int id);
		int Insert(Flower flower);
		bool Update(Flower flower);
	}
}
