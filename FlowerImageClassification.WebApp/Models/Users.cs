using System.Collections.Generic;

namespace FlowerImageClassification.WebApp.Models
{
	public class Users
	{
		public List<User> GetUsers()
		{
			return new List<User> { new User
				{
					Id = 1, FullName = "La Quoc Thang", Username = "admin", Password = "admin"
				}
			};
		}
	}
}