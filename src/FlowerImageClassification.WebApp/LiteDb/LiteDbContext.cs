using LiteDB;
using Microsoft.Extensions.Options;

namespace FlowerImageClassification.WebApp.LiteDb
{
	public class LiteDbContext : ILiteDbContext
	{
		public LiteDatabase Database { get; }

		public LiteDbContext(IOptions<LiteDbOptions> options)
		{
			Database = new LiteDatabase(options.Value.DatabaseLocation);
		}
	}
}
