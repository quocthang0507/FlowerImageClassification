﻿using FlowerImageClassification.WebApp.Models;
using LiteDB;
using System.Collections.Generic;
using System.Linq;

namespace FlowerImageClassification.WebApp.LiteDb
{
	public class LiteDbFlowerService : ILiteDbFlowerService
	{
		private LiteDatabase liteDb;

		public LiteDbFlowerService(ILiteDbContext liteDbContext)
		{
			liteDb = liteDbContext.Database;
		}

		public IEnumerable<Flower> FindAll() => liteDb.GetCollection<Flower>("Flower").FindAll();

		public Flower FindOne(int id) => liteDb.GetCollection<Flower>("Flower").
			Find(f => f.ID == id).FirstOrDefault();

		public int Insert(Flower flower) => liteDb.GetCollection<Flower>("Api").Insert(flower);

		public bool Update(Flower flower) => liteDb.GetCollection<Flower>("Api").Update(flower);

		public int Delete(int id) => liteDb.GetCollection<Flower>("Api").DeleteMany(f => f.ID == id);
	}
}
