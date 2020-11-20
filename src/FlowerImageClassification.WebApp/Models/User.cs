namespace FlowerImageClassification.WebApp.Models
{
	public class User
	{
		public int Id { get; set; }
		public string ActorName { get; set; }
		public string Username { get; set; }
		public string Password { get; set; }
		public string Role { get; set; }
		public string Token { get; set; }

		public User WithoutPassword()
		{
			if (this == null)
				return null;
			this.Password = null;
			return this;
		}
	}
}
