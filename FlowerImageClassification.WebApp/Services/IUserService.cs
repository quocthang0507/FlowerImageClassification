using System.Threading.Tasks;

namespace FlowerImageClassification.WebApp.Services
{
	public interface IUserService
	{
		Task<bool> IsValidUser(string username, string password);
	}
}
