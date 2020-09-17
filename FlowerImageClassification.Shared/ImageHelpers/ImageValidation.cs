using System;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;

namespace FlowerImageClassification.Shared.Models.ImageHelpers
{
	/// <summary>
	/// Validate byte[] of array is belonged to an image
	/// </summary>
	public static class ImageValidation
	{
		public static bool IsValidImage(this byte[] image)
		{
			var format = GetImageFormat(image);
			return format == ImageFormat.Jpeg || format == ImageFormat.Png;
		}

		/// <summary>
		/// http://www.mikekunz.com/image_file_header.html  
		/// </summary>
		/// <param name="image"></param>
		/// <returns></returns>
		public static ImageFormat GetImageFormat(byte[] bytes)
		{
			var bmp = Encoding.ASCII.GetBytes("BM");
			var gif = Encoding.ASCII.GetBytes("GIF");
			var png = new byte[] { 137, 80, 78, 71 };
			var tiff = new byte[] { 73, 73, 42 };
			var tiff2 = new byte[] { 77, 77, 42 };
			var jpeg = new byte[] { 255, 216, 255, 224 };
			var jpeg2 = new byte[] { 255, 216, 255, 225 };

			if (bmp.SequenceEqual(bytes.Take(bmp.Length)))
				return ImageFormat.Bmp;

			if (gif.SequenceEqual(bytes.Take(gif.Length)))
				return ImageFormat.Gif;

			if (png.SequenceEqual(bytes.Take(png.Length)))
				return ImageFormat.Png;

			if (tiff.SequenceEqual(bytes.Take(tiff.Length)))
				return ImageFormat.Tiff;

			if (tiff2.SequenceEqual(bytes.Take(tiff2.Length)))
				return ImageFormat.Tiff;

			if (jpeg.SequenceEqual(bytes.Take(jpeg.Length)))
				return ImageFormat.Jpeg;

			if (jpeg2.SequenceEqual(bytes.Take(jpeg2.Length)))
				return ImageFormat.Jpeg;

			return null;

		}
	}
}
