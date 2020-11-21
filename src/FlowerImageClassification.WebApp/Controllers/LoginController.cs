using FlowerImageClassification.WebApp.Models;
using FlowerImageClassification.WebApp.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace FlowerImageClassification.WebApp.Controllers
{
	public class LoginController : Controller
	{
		private readonly IUserService userService;
		private const string TOKEN = "token";
		private const string NAME = "Username";
		private const string ROLE = "Role";

		public LoginController(IUserService userService)
		{
			this.userService = userService;
		}

		public IActionResult Index()
		{
			return View();
		}


		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Login(LoginModel model)
		{
			if (ModelState.IsValid)
			{
				var user = userService.Authenticate(model.Username, model.Password);
				if (user == null)
				{
					ViewBag.Message = "Sai tên đăng nhập hoặc mật khẩu";
					return Index();
				}
				HttpContext.Session.Set(NAME, Encoding.UTF8.GetBytes(user.Username));
				HttpContext.Session.Set(ROLE, Encoding.UTF8.GetBytes(user.Role));
				HttpContext.Response.Cookies.Append(TOKEN, user.Token, new CookieOptions { HttpOnly = true });
				return RedirectToAction("Index", "Home");
			}
			return Unauthorized();
		}

		[Route("Logout")]
		public IActionResult Logout()
		{
			HttpContext.Session.Remove("Username");
			HttpContext.Session.Remove("Role");
			HttpContext.Response.Cookies.Delete(TOKEN);
			return RedirectToAction("Index", "Home");
		}
	}
}
