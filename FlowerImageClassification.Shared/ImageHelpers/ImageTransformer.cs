using FlowerImageClassification.Shared.Models.ImageHelpers;
using System;
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
		/// Transform byte[] to image object and save to file
		/// </summary>
		/// <param name="imageData"></param>
		/// <param name="path">Full path with file name</param>
		public static void ImageArrayToFile(byte[] imageData, string path)
		{
			ImageFormat format = ImageValidation.GetImageFormat(imageData);
			string ext = format == ImageFormat.Jpeg ? ".jpg" : ".png";
			string fileName = Path.GetRandomFileName().Split('.')[0];
			string filePath = Path.Combine(path, fileName + ext);
			File.WriteAllBytes(filePath, imageData);
		}
	}
}
