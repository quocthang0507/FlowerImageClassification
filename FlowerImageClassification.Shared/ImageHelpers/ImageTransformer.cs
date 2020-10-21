using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

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
		public static byte[] Base64ToByteArray(string base64String)
		{
			if (base64String.StartsWith("data:image"))
				return Convert.FromBase64String(base64String.Split(',')[1]);
			return null;
		}

		/// <summary>
		/// Transform byte[] to image object
		/// </summary>
		/// <param name="imageData"></param>
		/// <returns></returns>
		public static Image ByteArrayToImage(byte[] imageData)
		{
			Image image = null;
			using (MemoryStream ms = new MemoryStream(imageData))
			{
				image = Image.FromStream(ms);
			}
			return image;
		}

		/// <summary>
		/// Transform byte[] to image object and save to filename
		/// </summary>
		/// <param name="imageData"></param>
		/// <param name="path">Full path with file name</param>
		public static void ImageArrayToFile(byte[] imageData, string path)
		{
			var image = ByteArrayToImage(imageData);
			string fileName = Path.GetRandomFileName().Split('.')[0];
			image.Save(Path.Combine(path, fileName + ".jpg"), ImageFormat.Jpeg);
		}
	}
}
