namespace FlowerImageClassification.Shared.ImageSchema
{
	/// <summary>
	/// Class represents bytes of a single image data
	/// </summary>
	public class ImageDataInMemory
	{
		/// <summary>
		/// Bytes of image file
		/// </summary>
		public byte[] ImageBytes;

		/// <summary>
		/// Path to image file
		/// </summary>
		public string ImagePath { get; set; }

		/// <summary>
		/// Name of this image file
		/// </summary>
		public string Label { get; set; }

		public ImageDataInMemory(byte[] imageBytes, string imagePath, string label)
		{
			ImageBytes = imageBytes;
			ImagePath = imagePath;
			Label = label;
		}
	}
}
