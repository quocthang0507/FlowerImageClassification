using FlowerImageClassification.Shared.ImageHelpers;
using FlowerImageClassification.Shared.ImageSchema;
using FlowerImageClassification.Shared.Models.ImageHelpers;
using FlowerImageClassification.WebApp.LiteDb;
using FlowerImageClassification.WebApp.Models;
using FlowerImageClassification.WebApp.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.ML;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace FlowerImageClassification.WebApp.Controllers
{
	public class HomeController : Controller
	{
		public IConfiguration Configuration { get; }

		private readonly PredictionEnginePool<ImageDataInMemory, ImagePrediction> predictionEnginePool;
		private readonly ILiteDbFlowerService flowerService;
		private readonly ILogger<HomeController> logger;

		public HomeController(PredictionEnginePool<ImageDataInMemory, ImagePrediction> predictionEnginePool, IConfiguration configuration, ILogger<HomeController> logger, ILiteDbFlowerService flowerService)
		{
			// Get the ML Model Engine injected, for scoring
			this.predictionEnginePool = predictionEnginePool;
			this.Configuration = configuration;
			// Get other injected dependencies
			this.logger = logger;
			this.flowerService = flowerService;
		}

		public IActionResult Index()
		{
			return View();
		}

		[Route("Privacy")]
		public IActionResult Privacy()
		{
			return View();
		}

		[Route("About")]
		public IActionResult About()
		{
			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}

		[HttpGet]
		[Route("api/ClassifyImage")]
		public ActionResult<string> Get() => "Hello, World! From La Quoc Thang";

		[HttpGet]
		[ProducesResponseType(200)]
		[ProducesResponseType(400)]
		[Route("api/GetInfo/{name}")]
		public ActionResult<string> GetInfo(string name)
		{
			if (!ModelState.IsValid || name == null)
				return BadRequest("Bad request because of invalid model state or null parameter");
			try
			{
				string info = flowerService.GetInfoByName(name);
				return Ok(info);
			}
			catch (Exception)
			{
				return NotFound("Not found");
			}
		}

		[HttpPost]
		[ProducesResponseType(200)]
		[ProducesResponseType(400)]
		[Route("api/ClassifyImage")]
		public async Task<IActionResult> ClassifyImage(IFormFile imageFile)
		{
			if (!ModelState.IsValid || imageFile == null || imageFile.Length == 0)
				return BadRequest("Bad request because of invalid model state or null parameter");
			byte[] imageData = await Transformer.GetByteFromUploadedFile(imageFile);
			return Classify(imageData, imageFile.FileName);
		}

		[HttpPost]
		[ProducesResponseType(200)]
		[ProducesResponseType(400)]
		[Route("api/ClassifyBase64")]
		public async Task<IActionResult> ClassifyBase64(string base64image)
		{
			if (!ModelState.IsValid || base64image == null)
				return BadRequest("Bad request because of invalid model state or null parameter");
			byte[] imageData = await ImageTransformer.Base64ToByteArray(base64image);
			if (imageData == null)
				return BadRequest("Bad request because of an invalid base64 image");
			return Classify(imageData);
		}

		private IActionResult Classify(byte[] imageData, string filename = null)
		{
			// Check that the image is valid
			if (!imageData.IsValidImage())
				return StatusCode(StatusCodes.Status415UnsupportedMediaType);

			logger.LogInformation("Start processing image...");

			// Measure execution time
			Stopwatch watch = Stopwatch.StartNew();

			// Set the specific image data into the ImageInputData type used in the DataView
			ImageDataInMemory imageInputData = new ImageDataInMemory(imageData, null, null);

			// Predict code for provided image
			ImagePrediction prediction = predictionEnginePool.Predict(imageInputData);

			// Stop measuring time
			watch.Stop();
			long elapsedTime = watch.ElapsedMilliseconds;

			logger.LogInformation($"Image processed in {elapsedTime} miliseconds");

			// Predict the image's label with highest probability
			ImagePredictedLabelWithProbability bestPrediction = new ImagePredictedLabelWithProbability
			{
				PredictedLabel = prediction.PredictedLabel,
				Probability = prediction.Score.Max(),
				PredictionExecutionTime = elapsedTime,
				ImageID = filename
			};
			return Ok(bestPrediction);
		}

	}
}

