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
			ImageFormat format = GetImageFormat(image);
			return format == ImageFormat.Jpeg || format == ImageFormat.Png;
		}

		/// <summary>
		/// http://www.mikekunz.com/image_file_header.html  
		/// </summary>
		/// <param name="image"></param>
		/// <returns></returns>
		public static ImageFormat GetImageFormat(byte[] bytes)
		{
			byte[] bmp = Encoding.ASCII.GetBytes("BM");
			byte[] gif = Encoding.ASCII.GetBytes("GIF");
			byte[] png = new byte[] { 137, 80, 78, 71 };
			byte[] tiff = new byte[] { 73, 73, 42 };
			byte[] tiff2 = new byte[] { 77, 77, 42 };
			byte[] jpeg = new byte[] { 255, 216, 255, 224 };
			byte[] jpeg2 = new byte[] { 255, 216, 255, 225 };

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
