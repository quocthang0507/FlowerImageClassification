using FlowerImageClassification.Shared.Models.ImageHelpers;
using Microsoft.AspNetCore.Http;
using System.Drawing.Imaging;
using System.IO;
using System.Threading.Tasks;

namespace FlowerImageClassification.WebApp.Utilities
{
	public class Transformer
	{
		private static readonly string contributionPath = @"wwwroot\Contributions";

		/// <summary>
		/// Reads the uploaded file and transform to byte array
		/// </summary>
		/// <param name="imageFile"></param>
		/// <returns></returns>
		public static async Task<byte[]> GetByteFromUploadedFile(IFormFile imageFile)
		{
			MemoryStream memoryStream = new MemoryStream();

			// Asynchronously copies the content of the uploaded file
			await imageFile.CopyToAsync(memoryStream);

			// Check that the image is valid
			byte[] imageData = memoryStream.ToArray();
			return imageData;
		}

		/// <summary>
		/// Saves image data (in byte array) and returns it's random file name with extension type
		/// </summary>
		/// <param name="imageData"></param>
		/// <returns></returns>
		public static async Task<string> SaveByteToFile(byte[] imageData)
		{
			string ext = ImageValidation.GetImageFormat(imageData) != null ?
				(ImageValidation.GetImageFormat(imageData) == ImageFormat.Jpeg ? ".jpg" : ".png") : null;
			if (ext == null)
				return null;
			string filename = Path.GetRandomFileName().Split('.')[0] + ext;
			string filePath = Path.Combine(Directory.GetCurrentDirectory(), contributionPath, filename);
			using FileStream stream = new FileStream(filePath, FileMode.Create);
			await stream.WriteAsync(imageData);
			return filename;
		}
	}
}
