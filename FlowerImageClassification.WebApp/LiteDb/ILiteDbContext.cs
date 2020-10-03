using LiteDB;

namespace FlowerImageClassification.WebApp.LiteDb
{
	public interface ILiteDbContext
	{
		LiteDatabase Database { get; }
	}
}
