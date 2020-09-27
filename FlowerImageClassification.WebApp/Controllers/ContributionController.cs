using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
