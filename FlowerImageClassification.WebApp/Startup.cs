using FlowerImageClassification.Shared.ImageSchema;
using FlowerImageClassification.WebApp.LiteDb;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.ML;
using System.Drawing;
using System.IO;

namespace FlowerImageClassification.WebApp
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddControllersWithViews();

			// Register the PredictionEnginePool as a service in the IoC container for DI.
			services.AddPredictionEnginePool<ImageDataInMemory, ImagePrediction>().
				FromFile(Configuration["MLModel:MLModelFilePath"]);

			// WarmUpPredictionEnginePool(services);

			services.Configure<LiteDbOptions>(Configuration.GetSection("LiteDbOptions"));
			services.AddSingleton<ILiteDbContext, LiteDbContext>();
			services.AddTransient<ILiteDbFlowerService, LiteDbFlowerService>();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			else
			{
				app.UseExceptionHandler("/Home/Error");
			}
			app.UseStaticFiles();

			app.UseRouting();

			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllerRoute(
					name: "default",
					pattern: "{controller=Home}/{action=Index}/{id?}");
			});
		}

		private static void WarmUpPredictionEnginePool(IServiceCollection services)
		{
			//#1 - Simply get a Prediction Engine
			var predictionEnginePool = services.BuildServiceProvider().GetRequiredService<PredictionEnginePool<ImageDataInMemory, ImagePrediction>>();
			var predictionEngine = predictionEnginePool.GetPredictionEngine();
			predictionEnginePool.ReturnPredictionEngine(predictionEngine);

			// #2 - Predict
			// Get a sample image

			Image img = Image.FromFile(@"...");
			byte[] imageData;
			IFormFile imageFile;
			using (MemoryStream ms = new MemoryStream())
			{
				img.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
				//To byte[] (#1)
				imageData = ms.ToArray();

				//To FormFile (#2)
				imageFile = new FormFile(ms, 0, ms.Length, "BlackRose", "BlackRose.png");
			}

			var imageInputData = new ImageDataInMemory(imageBytes: imageData, label: null, imagePath: null);

			// Measure execution time.
			var watch = System.Diagnostics.Stopwatch.StartNew();

			var prediction = predictionEnginePool.Predict(imageInputData);

			// Stop measuring time.
			watch.Stop();
			var elapsedMs = watch.ElapsedMilliseconds;

		}
	}
}
