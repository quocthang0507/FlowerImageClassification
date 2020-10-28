using FlowerImageClassification.WebApp.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace FlowerImageClassification.WebApp.Services
{
	public class UserService : IUserService
	{
		private readonly ILogger<UserService> logger;

		public UserService(ILogger<UserService> logger)
		{
			this.logger = logger;
		}

		public async Task<bool> IsValidUser(string username, string password)
		{
			logger.LogInformation($"Validating user [{username}]");
			Users users = new Users();
			var list = users.GetUsers();
			var user = await Task.Run(
				() => list.SingleOrDefault(u =>
				u.Username.Equals(username, StringComparison.OrdinalIgnoreCase) &&
				u.Password.Equals(password)
			));
			return user == null;
		}
	}
}
