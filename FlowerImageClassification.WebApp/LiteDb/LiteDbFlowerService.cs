using FlowerImageClassification.WebApp.Models;
using LiteDB;
using System;
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

		/// <summary>
		/// Get all the flower from database
		/// </summary>
		/// <returns></returns>
		public IEnumerable<Flower> FindAll() => liteDb.GetCollection<Flower>().FindAll();

		/// <summary>
		/// Find and get the flower by id
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public Flower FindOne(int id) => liteDb.GetCollection<Flower>().
			Find(f => f.ID == id).FirstOrDefault();

		/// <summary>
		/// Insert a new flower to database
		/// </summary>
		/// <param name="flower"></param>
		/// <returns></returns>
		public int Insert(Flower flower) => liteDb.GetCollection<Flower>().Insert(flower);

		/// <summary>
		/// Update an existing flower
		/// </summary>
		/// <param name="flower"></param>
		/// <returns></returns>
		public bool Update(Flower flower) => liteDb.GetCollection<Flower>().Update(flower);

		/// <summary>
		/// Delete a flower by id
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public int Delete(int id) => liteDb.GetCollection<Flower>().DeleteMany(f => f.ID == id);

		/// <summary>
		/// Get flower information by English name
		/// </summary>
		/// <param name="englishName"></param>
		/// <returns></returns>
		public string GetInfoByName(string englishName) => 
			liteDb.GetCollection<Flower>().
			Find(f => f.EnglishName.Contains(englishName, StringComparison.OrdinalIgnoreCase)).
			FirstOrDefault().RichTextInfo;
	}
}
