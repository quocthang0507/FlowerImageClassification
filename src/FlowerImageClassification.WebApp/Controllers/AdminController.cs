﻿using FlowerImageClassification.WebApp.LiteDb;
using FlowerImageClassification.WebApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
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
			//imageset.SaveToFile(@"D:\GitHub\FlowerImageClassification\FlowerImageClassification.WebApp\_backup\");
			//imageset.RestoreImageSet(@"D:\GitHub\FlowerImageClassification\FlowerImageClassification.WebApp\_backup\");
		}

		public IActionResult Index()
		{
			if (!User.IsInRole(Role.Admin))
				return Forbid();
			IEnumerable<Flower> flowers = GetAll();
			return View(flowers);
		}

		[HttpGet]
		[ProducesResponseType(200)]
		[ProducesResponseType(400)]
		[Route("api/GetById/{id:int}")]
		public IActionResult GetById(int id)
		{
			if (!ModelState.IsValid)
				return BadRequest();
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
		[Route("api/Update")]
		public IActionResult Update([FromBody] Flower flower)
		{
			if (!ModelState.IsValid || flower == null)
				return BadRequest();
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
