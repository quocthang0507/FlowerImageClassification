using FlowerImageClassification.Shared.ImageHelpers;
using FlowerImageClassification.WebApp.LiteDb;
using FlowerImageClassification.WebApp.Models;
using FlowerImageClassification.WebApp.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace FlowerImageClassification.WebApp.Controllers
{
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
			return View();
		}

		//[HttpPost]
		//[ProducesResponseType(200)]
		//[ProducesResponseType(400)]
		//[Route("api/AddSentiment")]
		//public async Task<IActionResult> AddSentiment([FromBody] Sentiment sentiment, IFormFile imageFile)
		//{
		//	if (!ModelState.IsValid || imageFile == null || imageFile.Length == 0 || sentiment == null)
		//		return BadRequest("Bad request because of invalid model state or null paramter(s)");
		//	byte[] imageData = await Transformer.GetByteFromUploadedFile(imageFile);
		//	bool saved = await Transformer.SaveByteToFile(imageData);
		//	if (!saved)
		//		return BadRequest("Bad request because of invalid image");
		//	sentimentService.Insert(sentiment);
		//	return Ok();
		//}

	}
}
