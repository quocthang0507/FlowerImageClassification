using FlowerImageClassification.Shared.ImageHelpers;
using FlowerImageClassification.WebApp.LiteDb;
using FlowerImageClassification.WebApp.Models;
using FlowerImageClassification.WebApp.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FlowerImageClassification.WebApp.Controllers
{
	public class ContributionController : Controller
	{
		public IConfiguration Configuration { get; }

		private readonly ILiteDbSentimentService sentimentService;

		public ContributionController(IConfiguration configuration, ILiteDbSentimentService sentimentService)
		{
			Configuration = configuration;
			this.sentimentService = sentimentService;
		}

		[Authorize(Roles = Role.Expert)]
		public IActionResult Index()
		{
			IEnumerable<Sentiment> sentiments = GetAll();
			return View(sentiments);
		}

		[HttpPost]
		[ProducesResponseType(200)]
		[ProducesResponseType(400)]
		[Route("api/Contribution")]
		[AllowAnonymous]
		public async Task<IActionResult> SaveUserContribution(IFormFile imageFile, string predictedLabel)
		{
			if (!ModelState.IsValid || imageFile == null || imageFile.Length == 0 || (imageFile.Length / 1000) > 2048)
				return BadRequest("Bad request because of invalid model state or null parameter or too large");
			byte[] imageData = await Transformer.GetByteFromUploadedFile(imageFile);
			string filename = await Transformer.SaveByteToFile(imageData);
			if (filename == null)
				return BadRequest("The file which you uploaded can't save in server");
			Sentiment user = new Sentiment(filename, predictedLabel, DateTime.Now);
			sentimentService.Insert(user);
			return Ok(user);
		}

		[HttpPost]
		[ProducesResponseType(200)]
		[ProducesResponseType(400)]
		[Route("api/Base64Contribution")]
		[AllowAnonymous]
		public async Task<IActionResult> SaveUserBase64Contribution(string base64image, string predictedLabel)
		{
			if (!ModelState.IsValid || base64image == null)
				return BadRequest("Bad request because of invalid model state or null parameter");
			byte[] imageData = await ImageTransformer.Base64ToByteArray(base64image);
			if (imageData == null)
				return BadRequest("Bad request because of an invalid base64 image");
			string filename = await Transformer.SaveByteToFile(imageData);
			if (filename == null)
				return BadRequest("The file which you uploaded can't save in server");
			Sentiment user = new Sentiment(filename, predictedLabel, DateTime.Now);
			sentimentService.Insert(user);
			return Ok(user);
		}

		[HttpPost]
		[ProducesResponseType(200)]
		[ProducesResponseType(400)]
		[Route("api/UpdateLabel")]
		[Authorize(Roles = Role.Expert)]
		public IActionResult UpdateLabel(int id, string newLabel = "")
		{
			if (!ModelState.IsValid)
				return BadRequest("Bad request because of invalid model state");
			Sentiment sentiment = sentimentService.FindOne(id);
			if (sentiment == null)
				return NotFound("Not found a sentiment which have this id");
			sentiment.NewLabel = newLabel;
			sentimentService.Update(sentiment);
			return Ok(sentiment);
		}

		[HttpPost]
		[ProducesResponseType(200)]
		[ProducesResponseType(400)]
		[Authorize(Roles = Role.Admin)]
		[Route("api/MarkComplete")]
		public IActionResult MarkComplete(int id)
		{
			if (!ModelState.IsValid)
				return BadRequest("Bad request because of invalid model state");
			Sentiment sentiment = sentimentService.FindOne(id);
			if (sentiment == null)
				return NotFound("Not found a sentiment which have this id");
			sentiment.Visible = false;
			sentimentService.Update(sentiment);
			return Ok();
		}

		[HttpPost]
		[ProducesResponseType(200)]
		[ProducesResponseType(400)]
		[Authorize(Roles = Role.Admin)]
		[Route("api/UnmarkComplete")]
		public IActionResult UnmarkComplete(int id)
		{
			if (!ModelState.IsValid)
				return BadRequest("Bad request because of invalid model state");
			Sentiment sentiment = sentimentService.FindOne(id);
			if (sentiment == null)
				return NotFound("Not found a sentiment which have this id");
			sentiment.Visible = true;
			sentimentService.Update(sentiment);
			return Ok();
		}

		[HttpPost]
		[ProducesResponseType(200)]
		[ProducesResponseType(400)]
		[Authorize(Roles = Role.Admin)]
		public IActionResult DeleteImage(int id)
		{
			if (!ModelState.IsValid)
				return BadRequest("Bad request because of invalid model state");
			if (sentimentService.Delete(id))
				return Ok();
			else
				return NotFound("Not found a sentiment which have this id");
		}

		[HttpGet]
		[ProducesResponseType(200)]
		[ProducesResponseType(400)]
		[Authorize(Roles = Role.Admin)]
		[Route("api/GetSentiment/{id:int}")]
		public IActionResult GetSentimentByID(int id)
		{
			if (!ModelState.IsValid)
				return BadRequest("Bad request because of invalid model state");
			Sentiment sentiment = GetOne(id);
			if (sentiment == null)
				return NotFound("Not found a sentiment which have this id");
			return Ok(sentiment);
		}

		private IEnumerable<Sentiment> GetAll() => sentimentService.FindAll();

		private Sentiment GetOne(int id) => sentimentService.FindOne(id);
	}
}
