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

		public static async Task<byte[]> GetByteFromUploadedFile(IFormFile imageFile)
		{
			MemoryStream memoryStream = new MemoryStream();

			// Asynchronously copies the content of the uploaded file
			await imageFile.CopyToAsync(memoryStream);

			// Check that the image is valid
			byte[] imageData = memoryStream.ToArray();
			return imageData;
		}

		public static async Task<bool> SaveByteToFile(byte[] imageData)
		{
			string ext = ImageValidation.GetImageFormat(imageData) != null ?
				(ImageValidation.GetImageFormat(imageData) == ImageFormat.Jpeg ? ".jpg" : ".png") : null;
			if (ext == null)
				return false;
			string filePath = Path.Combine(Directory.GetCurrentDirectory(), contributionPath, Path.GetRandomFileName().Split('.')[0] + ext);
			using FileStream stream = new FileStream(filePath, FileMode.Create);
			await stream.WriteAsync(imageData);
			return true;
		}


	}
}
