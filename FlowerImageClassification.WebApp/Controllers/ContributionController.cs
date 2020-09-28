using Microsoft.AspNetCore.Mvc;

namespace FlowerImageClassification.WebApp.Controllers
{
	public class ContributionController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
