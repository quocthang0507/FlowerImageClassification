using FlowerImageClassification.WebApp.LiteDb;
using FlowerImageClassification.WebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
			FlowerDataset imageset = new FlowerDataset((LiteDbFlowerService)flowerService);
			imageset.InitializeImageSet();
		}

		public IActionResult Index()
		{
			var flowers = GetAll();
			return View(flowers);
		}

		[HttpGet]
		[ProducesResponseType(200)]
		[ProducesResponseType(400)]
		[Route("api/GetById/{id:int}")]
		public IActionResult GetById(int id)
		{
			var result = flowerService.FindOne(id);
			if (result != null)
				return Ok(result);
			else
				return NotFound();
		}

		[HttpPost]
		[ProducesResponseType(200)]
		[ProducesResponseType(400)]
		[Route("api/update")]
		public IActionResult Update([FromBody]Flower flower)
		{
			var result = flowerService.Update(flower);
			if (result)
				return Ok();
			return BadRequest();
		}

		private IEnumerable<Flower> GetAll()
		{
			return flowerService.FindAll();
		}
	}
}
