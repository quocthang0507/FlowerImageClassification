using FlowerImageClassification.WebApp.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace FlowerImageClassification.WebApp.Services
{
	public class UserService : IUserService
	{
		private readonly ILogger<UserService> logger;
		private readonly AppSettings appSettings;

		public UserService(ILogger<UserService> logger, IOptions<AppSettings> appSettings)
		{
			this.logger = logger;
			this.appSettings = appSettings.Value;
		}

		public User Authenticate(string username, string password)
		{
			logger.LogInformation($"Validating user [{username}]");
			Accounts users = new Accounts();
			List<User> list = users.GetAccounts();
			User user = list.SingleOrDefault(u =>
				 u.Username.Equals(username, StringComparison.OrdinalIgnoreCase) &&
				 u.Password.Equals(password));
			if (user == null)
				return null;

			var tokenHandler = new JwtSecurityTokenHandler();
			var key = Encoding.ASCII.GetBytes(appSettings.Secret);
			var tokenDescriptor = new SecurityTokenDescriptor
			{
				Subject = new ClaimsIdentity(new Claim[]
				{
					new Claim(ClaimTypes.Name, user.Username),
					new Claim(ClaimTypes.Role, user.Role)
				}),
				Expires = DateTime.UtcNow.AddDays(7),
				SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
			};
			var token = tokenHandler.CreateToken(tokenDescriptor);
			user.Token = tokenHandler.WriteToken(token);

			return user.WithoutPassword();
		}
	}
}
