using FlowerImageClassification.Shared.ImageSchema;
using FlowerImageClassification.WebApp.LiteDb;
using FlowerImageClassification.WebApp.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.ML;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Threading.Tasks;

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
			services.AddCors();

			services.AddControllersWithViews();

			services.AddSession();

			// Register the PredictionEnginePool as a service in the IoC container for DI.
			services.AddPredictionEnginePool<ImageDataInMemory, ImagePrediction>().
				FromFile(Configuration["MLModel:MLModelFilePath"]);

			var appSettingsSection = Configuration.GetSection("AppSettings");

			services.Configure<LiteDbOptions>(Configuration.GetSection("LiteDbOptions"));
			services.Configure<AppSettings>(appSettingsSection);

			services.AddSingleton<ILiteDbContext, LiteDbContext>();
			services.AddTransient<ILiteDbFlowerService, LiteDbFlowerService>();
			services.AddTransient<ILiteDbSentimentService, LiteDbSentimentService>();

			var appSettings = appSettingsSection.Get<AppSettings>();
			var key = Encoding.UTF8.GetBytes(appSettings.Secret);
			services.AddAuthentication(options =>
			{
				options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
			})
			.AddJwtBearer(options =>
			{
				options.RequireHttpsMetadata = false;
				options.SaveToken = true;
				options.TokenValidationParameters = new TokenValidationParameters
				{
					ValidateIssuerSigningKey = true,
					IssuerSigningKey = new SymmetricSecurityKey(key),
					ValidateIssuer = false,
					ValidateAudience = false
				};
				options.Events = new JwtBearerEvents
				{
					OnMessageReceived = context =>
					{
						context.Token = context.Request.Cookies["token"];
						return Task.CompletedTask;
					}
				};
			});

			services.AddScoped<IUserService, UserService>();
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

			app.UseCors(x => x
				.AllowAnyOrigin()
				.AllowAnyMethod()
				.AllowAnyHeader());

			app.UseAuthentication();

			app.UseAuthorization();

			app.UseSession();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllerRoute(
					name: "default",
					pattern: "{controller=Home}/{action=Index}/{id?}");
			});

		}
	}
}
