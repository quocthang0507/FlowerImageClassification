using Microsoft.AspNetCore.Mvc;

namespace FlowerImageClassification.WebApp.Controllers
{
	public class CameraController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
