using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Server.HttpSys;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace FlowerImageClassification.WebApp.Services
{
	public class BasicAuthenticationFilter : IAuthorizationFilter
	{
		private readonly string realm;

		public BasicAuthenticationFilter(string realm)
		{
			this.realm = realm;
			if (string.IsNullOrWhiteSpace(realm))
				throw new ArgumentNullException(nameof(realm), @"Please provide a non-empty realm value");
		}

		public async void OnAuthorization(AuthorizationFilterContext context)
		{
			try
			{
				string authHeader = context.HttpContext.Request.Headers["Authorization"];
				if (authHeader != null)
				{
					AuthenticationHeaderValue authHeaderValue = AuthenticationHeaderValue.Parse(authHeader);
					if (authHeaderValue.Scheme.Equals(AuthenticationSchemes.Basic.ToString(), StringComparison.OrdinalIgnoreCase))
					{
						string[] credentials = Encoding.UTF8.GetString(Convert.FromBase64String(authHeaderValue.Parameter ?? string.Empty)).Split(':', 2);
						if (credentials.Length == 2)
						{
							bool successful = await IsAuthorized(context, credentials[0], credentials[1]);
							if (successful)
							{
								return;
							}
						}
					}
				}
				ReturnUnauthorizedResult(context);
			}
			catch (FormatException)
			{
				ReturnUnauthorizedResult(context);
			}
		}

		private void ReturnUnauthorizedResult(AuthorizationFilterContext context)
		{
			context.HttpContext.Response.Headers["WWW-Authenticate"] = $"Basic realm=\"{realm}\"";
			context.Result = new UnauthorizedResult();
		}

		private async Task<bool> IsAuthorized(AuthorizationFilterContext context, string username, string password)
		{
			IUserService userService = context.HttpContext.RequestServices.GetRequiredService<IUserService>();
			return await userService.IsValidUser(username, password);
		}
	}
}
