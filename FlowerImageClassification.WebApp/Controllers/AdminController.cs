using FlowerImageClassification.WebApp.LiteDb;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

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
			return View();
		}
	}
}
