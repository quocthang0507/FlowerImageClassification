using System.Collections.Generic;
using System.Linq;

namespace FlowerImageClassification.WebApp.Models
{
	public class Users
	{
		public List<User> GetUsers()
		{
			return new List<User> {
				new User
				{
					Id = 1, ActorName = "Quan tri vien", Username = "admin", Password = "admin", Role = Role.Admin
				},
				new User
				{
					Id = 2, ActorName = "Chuyen gia gan nhan", Username = "expert", Password = "expert", Role = Role.Expert
				}
			};
		}

		public List<User> GetUsersWithoutPass()
		{
			return GetUsers().Select(u => u.WithoutPassword()).ToList();
		}
	}
}