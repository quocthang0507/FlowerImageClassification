namespace FlowerImageClassification.Shared.Image
{
	/// <summary>
	/// Class represents a single image data
	/// </summary>
	public class ImageData
	{
		/// <summary>
		/// Path to image file
		/// </summary>
		public string ImagePath { get; set; }
		/// <summary>
		/// Name of this image file
		/// </summary>
		public string Label { get; set; }

		public ImageData(string imagePath, string label)
		{
			ImagePath = imagePath;
			Label = label;
		}

	}
}
