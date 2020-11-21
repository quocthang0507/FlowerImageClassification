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
				HttpContext.Session.Set("Username", Encoding.UTF8.GetBytes(user.Username));
				HttpContext.Session.Set("Role", Encoding.UTF8.GetBytes(user.Role));
				HttpContext.Response.Cookies.Append("token", user.Token, new CookieOptions { HttpOnly = true });
				return RedirectToAction("Index", "Home");
			}
			return Unauthorized();
		}
	}
}
