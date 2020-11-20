using FlowerImageClassification.Shared.ImageHelpers;
using FlowerImageClassification.WebApp.LiteDb;
using FlowerImageClassification.WebApp.Models;
using FlowerImageClassification.WebApp.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FlowerImageClassification.WebApp.Controllers
{
	[Authorize(Roles = Role.Expert)]
	public class ContributionController : Controller
	{
		public IConfiguration Configuration { get; }

		private readonly ILiteDbSentimentService sentimentService;
		private readonly ILogger<ContributionController> logger;

		public ContributionController(IConfiguration configuration, ILiteDbSentimentService sentimentService, ILogger<ContributionController> logger)
		{
			Configuration = configuration;
			this.sentimentService = sentimentService;
			this.logger = logger;
		}

		public IActionResult Index()
		{
			IEnumerable<Sentiment> sentiments = GetAll();
			return View(sentiments);
		}

		private IEnumerable<Sentiment> GetAll() => sentimentService.FindAll();

		[HttpPost]
		[ProducesResponseType(200)]
		[ProducesResponseType(400)]
		[Route("api/Contribution")]
		public async Task<IActionResult> SaveUserContribution(IFormFile imageFile, string predictedLabel)
		{
			if (!ModelState.IsValid || imageFile == null || imageFile.Length == 0 || (imageFile.Length / 1000) > 2048)
				return BadRequest("Bad request because of invalid model state or null parameter or too large");
			byte[] imageData = await Transformer.GetByteFromUploadedFile(imageFile);
			string filename = await Transformer.SaveByteToFile(imageData);
			if (filename == null)
				return BadRequest("The file which you uploaded can't save in server");
			Sentiment user = new Sentiment(filename, predictedLabel, 0, 0);
			sentimentService.Insert(user);
			return Ok(user);
		}

		[HttpPost]
		[ProducesResponseType(200)]
		[ProducesResponseType(400)]
		[Route("api/Base64Contribution")]
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
			Sentiment user = new Sentiment(filename, predictedLabel, 0, 0);
			sentimentService.Insert(user);
			return Ok(user);
		}

		[HttpPost]
		[ProducesResponseType(200)]
		[ProducesResponseType(400)]
		[Route("api/Vote")]
		public IActionResult VoteSentiment(int id, bool like = false, bool dislike = false)
		{
			if (!ModelState.IsValid || like == dislike)
				return BadRequest("Bad request because of invalid model state or invalid vote");
			Sentiment sentiment = sentimentService.FindOne(id);
			if (sentiment == null)
				return NotFound("Not found a sentiment which have this id");
			if (like)
				sentiment.LikeNumber++;
			else
				sentiment.DislikeNumber++;
			sentimentService.Update(sentiment);
			return Ok(sentiment);
		}
	}
}
