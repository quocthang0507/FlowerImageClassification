using FlowerImageClassification.Shared.ImageHelpers;
using FlowerImageClassification.Shared.ImageSchema;
using FlowerImageClassification.Shared.Models.ImageHelpers;
using FlowerImageClassification.WebApp.LiteDb;
using FlowerImageClassification.WebApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.ML;
using System;
using System.Diagnostics;
using System.IO;
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
		private string contributionPath = "Contributions/";

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
		public ActionResult<string> Get() => "Hello World";

		[HttpGet]
		[Route("api/GetInfo/{name}")]
		public ActionResult<string> GetInfo(string name)
		{
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
			if (imageFile == null || imageFile.Length == 0)
				return BadRequest();
			var imageData = await ReceiveFile(imageFile);
			return Classify(imageData, imageFile.FileName);
		}

		[HttpPost]
		[ProducesResponseType(200)]
		[ProducesResponseType(400)]
		[Route("api/CollectAndClassifyImage")]
		public async Task<IActionResult> CollectAndClassifyImage(IFormFile imageFile)
		{
			if (imageFile == null || imageFile.Length == 0)
				return BadRequest();
			var imageData = await ReceiveFile(imageFile);
			return Classify(imageData, imageFile.FileName, true);
		}

		[HttpPost]
		[ProducesResponseType(200)]
		[ProducesResponseType(400)]
		[Route("api/ClassifyImageBase64")]
		public IActionResult ClassifyImageBase64(string base64image)
		{
			var imageData = ImageTransformer.Base64ToByteArray(base64image);
			if (imageData == null)
				return BadRequest();
			return Classify(imageData);
		}

		[HttpPost]
		[ProducesResponseType(200)]
		[ProducesResponseType(400)]
		[Route("api/CollectAndClassifyImageBase64")]
		public IActionResult CollectAndClassifyImageBase64(string base64image)
		{
			var imageData = ImageTransformer.Base64ToByteArray(base64image);
			if (imageData == null)
				return BadRequest();
			return Classify(imageData, null, true);
		}

		private async Task<byte[]> ReceiveFile(IFormFile imageFile)
		{
			var memoryStream = new MemoryStream();

			// Asynchronously copies the content of the uploaded file
			await imageFile.CopyToAsync(memoryStream);

			// Check that the image is valid
			byte[] imageData = memoryStream.ToArray();
			return imageData;
		}

		private IActionResult Classify(byte[] imageData, string filename = null, bool savedFile = false)
		{
			// Check that the image is valid
			if (!imageData.IsValidImage())
				return StatusCode(StatusCodes.Status415UnsupportedMediaType);

			if (savedFile) ImageTransformer.ImageArrayToFile(imageData, contributionPath);

			logger.LogInformation("Start processing image...");

			// Measure execution time
			var watch = Stopwatch.StartNew();

			// Set the specific image data into the ImageInputData type used in the DataView
			var imageInputData = new ImageDataInMemory(imageData, null, null);

			// Predict code for provided image
			var prediction = predictionEnginePool.Predict(imageInputData);

			// Stop measuring time
			watch.Stop();
			var elapsedTime = watch.ElapsedMilliseconds;
			logger.LogInformation($"Image processed in {elapsedTime} miliseconds");

			// Predict the image's label with highest probability
			var bestPrediction = new ImagePredictedLabelWithProbability
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

