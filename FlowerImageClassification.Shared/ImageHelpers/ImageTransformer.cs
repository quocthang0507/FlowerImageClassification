using System;
using System.Drawing;
using System.IO;

namespace FlowerImageClassification.Shared.ImageHelpers
{
	public class ImageTransformer
	{
		public static byte[] Base64ToByteArray(string base64String)
		{
			if (base64String.StartsWith("data:image"))
				return Convert.FromBase64String(base64String.Split(',')[1]);
			return null;
		}

		public static Image ByteArrayToImage(byte[] imageData)
		{
			Image returnImage = null;
			using (MemoryStream ms = new MemoryStream(imageData))
			{
				returnImage = Image.FromStream(ms);
			}
			return returnImage;
		}
	}
}
