using FlowerImageClassification.WebApp.Models;

namespace FlowerImageClassification.WebApp.Services
{
	public interface IUserService
	{
		User Authenticate(string username, string password);
	}
}
