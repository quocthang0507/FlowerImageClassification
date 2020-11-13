using FlowerImageClassification.WebApp.LiteDb;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace FlowerImageClassification.WebApp.Models
{
	public class FlowerDataset
	{
		private LiteDbFlowerService flowerService;

		public IEnumerable<Flower> ImageSet { get; set; }

		public FlowerDataset(LiteDbFlowerService flowerService)
		{
			this.flowerService = flowerService;
			ImageSet = new Flower[]
			{
				new Flower{ID=1,VietnameseName="Hoa cúc",EnglishName="daisy",RichTextInfo="",Thumbnail="/thumbnail/daisy.jpg"},
				new Flower{ID=2,VietnameseName="Hoa bồ công anh",EnglishName="dandelion",RichTextInfo="",Thumbnail="/thumbnail/dandelion.jpg"},
				new Flower{ID=3,VietnameseName="Hoa hồng",EnglishName="rose",RichTextInfo="",Thumbnail="/thumbnail/rose.jpg"},
				new Flower{ID=4,VietnameseName="Hoa hướng dương",EnglishName="sunflower",RichTextInfo="",Thumbnail="/thumbnail/sunflower.jpg"},
				new Flower{ID=5,VietnameseName="Hoa uất kim hương",EnglishName="tulip",RichTextInfo="",Thumbnail="/thumbnail/tulip.jpg"}
			};
		}

		public void InitializeImageSet()
		{
			foreach (Flower flower in ImageSet)
			{
				if (flowerService.FindOne(flower.ID) == null)
					flowerService.Insert(flower);
			}
		}

		public void SaveToFile(string pathToFolder)
		{
			IEnumerable<Flower> db = flowerService.FindAll();
			foreach (Flower flower in db)
			{
				string fullPath = Path.Combine(pathToFolder, flower.EnglishName + ".txt");
				using (StreamWriter sw = new StreamWriter(fullPath, false, Encoding.UTF8))
				{
					sw.WriteLine(flower.ID);
					sw.WriteLine(flower.VietnameseName);
					sw.WriteLine(flower.EnglishName);
					sw.WriteLine(flower.RichTextInfo);
					sw.WriteLine(flower.Thumbnail);
				}
			}
		}

		public void RestoreImageSet(string pathToFolder)
		{
			string[] files = Directory.GetFiles(pathToFolder, "*.txt", SearchOption.TopDirectoryOnly);
			foreach (string file in files)
			{
				using (StreamReader sr = new StreamReader(file))
				{
					string data = sr.ReadToEnd();
					string[] arr = data.Split("\r\n");
					Flower flower = new Flower()
					{
						ID = int.Parse(arr[0]),
						VietnameseName = arr[1],
						EnglishName = arr[2],
						RichTextInfo = arr[3],
						Thumbnail = arr[4]
					};
					flowerService.InsertOrUpdateIfExisted(flower);
				}
			}
		}
	}
}
