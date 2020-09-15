namespace FlowerImageClassification.Shared.Image
{
	/// <summary>
	/// Class represents bytes of a single image data
	/// </summary>
	public class ImageDataInMemory
	{
		/// <summary>
		/// Bytes of image file
		/// </summary>
		public readonly byte[] ImageBytes;

		/// <summary>
		/// Basic info of image file
		/// </summary>
		public readonly ImageData ImageData;

		public ImageDataInMemory(byte[] imageBytes, ImageData imageData)
		{
			ImageBytes = imageBytes;
			ImageData = imageData;
		}
	}
}
