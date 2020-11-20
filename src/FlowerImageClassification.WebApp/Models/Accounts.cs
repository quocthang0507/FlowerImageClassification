using System.Collections.Generic;
using System.Linq;

namespace FlowerImageClassification.WebApp.Models
{
	public class Accounts
	{
		public List<User> GetAccounts()
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

		public List<User> GetAccWithoutPass()
		{
			return GetAccounts().Select(u => u.WithoutPassword()).ToList();
		}
	}
}