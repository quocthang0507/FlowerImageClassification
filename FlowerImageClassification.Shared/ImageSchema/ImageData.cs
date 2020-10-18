using Microsoft.ML.Data;

namespace FlowerImageClassification.Shared.ImageSchema
{
	/// <summary>
	/// Class represents a single image data
	/// </summary>
	public class ImageData
	{
		/// <summary>
		/// Path to image file
		/// </summary>
		[ColumnName("ImagePath"), LoadColumn(1)]
		public string ImagePath { get; set; }

		/// <summary>
		/// Name of this image file
		/// </summary>
		[ColumnName("Label"), LoadColumn(0)]
		public string Label { get; set; }

		public ImageData(string imagePath, string label)
		{
			ImagePath = imagePath;
			Label = label;
		}

	}
}
