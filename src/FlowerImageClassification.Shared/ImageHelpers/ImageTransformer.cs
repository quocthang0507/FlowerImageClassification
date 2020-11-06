using System;
using System.Threading.Tasks;

namespace FlowerImageClassification.Shared.ImageHelpers
{
	/// <summary>
	/// Image transformer class
	/// </summary>
	public class ImageTransformer
	{
		/// <summary>
		/// Transform base64 string to byte[]
		/// </summary>
		/// <param name="base64String"></param>
		/// <returns></returns>
		public async static Task<byte[]> Base64ToByteArray(string base64String)
		{
			if (base64String.StartsWith("data:image"))
				return Convert.FromBase64String(base64String.Split(',')[1]);
			return null;
		}
	}
}
