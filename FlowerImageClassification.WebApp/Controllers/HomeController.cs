﻿using FlowerImageClassification.Shared.Image;
using FlowerImageClassification.Shared.Models.ImageHelpers;
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
		private readonly ILogger<HomeController> _logger;

		public HomeController(PredictionEnginePool<ImageDataInMemory, ImagePrediction> predictionEnginePool, IConfiguration configuration, ILogger<HomeController> logger)
		{
			// Get the ML Model Engine injected, for scoring
			this.predictionEnginePool = predictionEnginePool;
			this.Configuration = configuration;
			// Get other injected dependencies
			_logger = logger;
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
		public ActionResult<string> Get()
		=> "Hello World";

		[HttpPost]
		[ProducesResponseType(200)]
		[ProducesResponseType(400)]
		[Route("api/ClassifyImage")]
		public async Task<IActionResult> ClassifyImage(IFormFile imageFile)
		{
			if (imageFile == null || imageFile.Length == 0)
				return BadRequest();
			var memoryStream = new MemoryStream();

			// Asynchronously copies the content of the uploaded file
			await imageFile.CopyToAsync(memoryStream);

			// Check that the image is valid
			byte[] imageData = memoryStream.ToArray();
			return Classify(imageData, imageFile.FileName);
		}

		[HttpPost]
		[ProducesResponseType(200)]
		[ProducesResponseType(400)]
		[Route("api/ClassifyImageBase64")]
		public IActionResult ClassifyImageBase64(string base64image)
		{
			var imageData = Base64ToByteArray(base64image);
			if (imageData == null)
				return BadRequest();
			return Classify(imageData, null);
		}

		private IActionResult Classify(byte[] imageData, string filename = null)
		{
			if (!imageData.IsValidImage())
				return StatusCode(StatusCodes.Status415UnsupportedMediaType);
			_logger.LogInformation("Start processing image...");

			// Measure execution time
			var watch = Stopwatch.StartNew();

			// Set the specific image data into the ImageInputData type used in the DataView
			var imageInputData = new ImageDataInMemory(imageData, null, null);

			// Predict code for provided image
			var prediction = predictionEnginePool.Predict(imageInputData);

			// Stop measuring time
			watch.Stop();
			var elapsedTime = watch.ElapsedMilliseconds;
			_logger.LogInformation($"Image processed in {elapsedTime} miliseconds");

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

		private byte[] Base64ToByteArray(string base64String)
		{
			if (base64String.StartsWith("data:image"))
				return Convert.FromBase64String(base64String.Split(',')[1]);
			return null;
		}
	}
}

