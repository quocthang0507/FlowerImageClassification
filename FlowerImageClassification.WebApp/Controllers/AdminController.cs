using FlowerImageClassification.WebApp.LiteDb;
using FlowerImageClassification.WebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace FlowerImageClassification.WebApp.Controllers
{
	public class AdminController : Controller
	{
		private readonly ILiteDbFlowerService flowerService;

		public AdminController(ILiteDbFlowerService flowerService)
		{
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
			Flower result;
			try
			{
				result = flowerService.FindOne(id);
				if (result != null)
					return Ok(result);
			}
			catch (Exception)
			{
				return BadRequest();
			}
			return NotFound();
		}

		[HttpPost]
		[ProducesResponseType(200)]
		[ProducesResponseType(400)]
		[Route("api/update")]
		public IActionResult Update([FromBody] Flower flower)
		{
			bool result;
			try
			{
				result = flowerService.Update(flower);
				if (result)
					return Ok(result);
			}
			catch (Exception)
			{
				return BadRequest();
			}
			return NotFound();
		}

		private IEnumerable<Flower> GetAll()
		{
			return flowerService.FindAll();
		}
	}
}
