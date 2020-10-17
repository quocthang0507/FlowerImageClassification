using FlowerImageClassification.WebApp.LiteDb;
using System.Collections.Generic;

namespace FlowerImageClassification.WebApp.Models
{
	public class FlowerDataset
	{
		public IEnumerable<Flower> Imageset { get; set; }

		private LiteDbFlowerService flowerService;

		public FlowerDataset(LiteDbFlowerService flowerService)
		{
			this.flowerService = flowerService;
			Imageset = new Flower[]
			{
				new Flower{ID=1,VietnameseName="Hoa cẩm chướng",EnglishName="Carnation",RichTextInfo="",Thumbnail="/thumbnail/carnation.jpg"},
				new Flower{ID=2,VietnameseName="Hoa cẩm tú cầu",EnglishName="Hydrangea",RichTextInfo="",Thumbnail="/thumbnail/hydrangea.jpg"},
				new Flower{ID=3,VietnameseName="Hoa phượng tím",EnglishName="Jacaranda",RichTextInfo="",Thumbnail="/thumbnail/jacaranda.jpg"},
				new Flower{ID=4,VietnameseName="Hoa oải hương",EnglishName="Lavender",RichTextInfo="",Thumbnail="/thumbnail/lavender.jpg"},
				new Flower{ID=5,VietnameseName="Hoa lay ơn",EnglishName="Gladiolus",RichTextInfo="",Thumbnail="/thumbnail/gladiolus.jpg"},
				new Flower{ID=6,VietnameseName="Hoa loa kèn",EnglishName="Lily",RichTextInfo="",Thumbnail="/thumbnail/lily.jpg"},
				new Flower{ID=7,VietnameseName="Hoa cát tường",EnglishName="Lisianthus",RichTextInfo="",Thumbnail="/thumbnail/lisianthus.jpg"},
				new Flower{ID=8,VietnameseName="Hoa hồng",EnglishName="Rose",RichTextInfo="",Thumbnail="/thumbnail/rose.jpg"},
				new Flower{ID=9,VietnameseName="Hoa hướng dương",EnglishName="Sunflower",RichTextInfo="",Thumbnail="/thumbnail/sunflower.jpg"},
				new Flower{ID=10,VietnameseName="Hoa uất kim hương",EnglishName="Tulip",RichTextInfo="",Thumbnail="/thumbnail/tulips.jpg"}
			};
		}

		public void InitializeImageSet()
		{
			foreach (var flower in Imageset)
			{
				if (flowerService.FindOne(flower.ID) == null)
					flowerService.Insert(flower);
			}
		}

	}
}
