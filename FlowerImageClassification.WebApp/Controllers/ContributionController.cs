using FlowerImageClassification.WebApp.LiteDb;
using FlowerImageClassification.WebApp.Models;
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

		private readonly ILiteDbSentimentService flowerService;
		private readonly ILogger<ContributionController> logger;

		public ContributionController(IConfiguration configuration, ILiteDbSentimentService flowerService, ILogger<ContributionController> logger)
		{
			Configuration = configuration;
			this.flowerService = flowerService;
			this.logger = logger;
		}

		public IActionResult Index()
		{
			return View();
		}

		[HttpPost]
		[ProducesResponseType(200)]
		[ProducesResponseType(400)]
		[Route("api/AddSentiment")]
		public async Task<IActionResult> AddSentiment([FromBody] Sentiment sentiment, IFormFile imageFile)
		{
			if (!ModelState.IsValid || imageFile == null || imageFile.Length == 0)
				return BadRequest();
			return Ok();
		}

		[HttpPost]
		[ProducesResponseType(200)]
		[ProducesResponseType(400)]
		[Route("api/AddSentimentBase64")]
		public async Task<IActionResult> AddSentimentWithBase64([FromBody] Sentiment sentiment, string base64)
		{
			if (!ModelState.IsValid || sentiment == null)
				return BadRequest();
			return Ok();
		}

	}
}
