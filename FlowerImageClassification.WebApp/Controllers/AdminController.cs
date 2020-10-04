using FlowerImageClassification.WebApp.LiteDb;
using FlowerImageClassification.WebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace FlowerImageClassification.WebApp.Controllers
{
	public class AdminController : Controller
	{
		private readonly ILogger<AdminController> logger;
		private readonly ILiteDbFlowerService flowerService;

		public AdminController(ILogger<AdminController> logger, ILiteDbFlowerService flowerService)
		{
			this.logger = logger;
			this.flowerService = flowerService;
		}

		public IActionResult Index()
		{
			FlowerDataset imageset = new FlowerDataset((LiteDbFlowerService)flowerService);
			imageset.InitializeImageSet();

			var flowers = GetAll();
			return View(flowers);
		}

		private IEnumerable<Flower> GetAll()
		{
			return flowerService.FindAll();
		}
	}
}
