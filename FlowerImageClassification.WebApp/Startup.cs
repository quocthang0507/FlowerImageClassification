using FlowerImageClassification.Shared.ImageSchema;
using FlowerImageClassification.WebApp.LiteDb;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.ML;

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
	}
}
