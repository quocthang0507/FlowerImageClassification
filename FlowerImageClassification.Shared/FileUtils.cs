using FlowerImageClassification.Shared.ImageSchema;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace FlowerImageClassification.Shared
{
	/// <summary>
	/// File utilities
	/// </summary>
	public class FileUtils
	{
		/// <summary>
		/// Load images from a directory and it's subfolders
		/// and put name as label to these files.
		/// </summary>
		/// <param name="pathToImageFolder"></param>
		/// <param name="nameAsLabel"></param>
		/// <returns></returns>
		public static IEnumerable<ImageData> LoadImagesFromDirectory(string pathToImageFolder, bool nameAsLabel = true)
		{
			var imagesPath = Directory.
				GetFiles(pathToImageFolder, "*", SearchOption.AllDirectories).
				Where(file => Path.GetExtension(file).Contains("jpg") || Path.GetExtension(file).Contains("png"));
			return nameAsLabel ?
				imagesPath.Select(path => new ImageData(path, Directory.GetParent(path).Name)) :
				imagesPath.Select(path => new ImageData(path, Path.GetFileName(path)));
		}

		/// <summary>
		/// Derivate from LoadImagesFromDirectory method
		/// but include a images's array of bytes
		/// </summary>
		/// <param name="pathToImageFolder"></param>
		/// <param name="nameAsLabel"></param>
		/// <returns></returns>
		public static IEnumerable<ImageDataInMemory> LoadImagesFromDirectoryInMemory(string pathToImageFolder, bool nameAsLabel = true)
			=> LoadImagesFromDirectory(pathToImageFolder, nameAsLabel).
				Select(image => new ImageDataInMemory(
					File.ReadAllBytes(image.ImagePath),
					image.ImagePath, image.Label));

		/// <summary>
		/// Get an absolute path from a relative path
		/// </summary>
		/// <param name="assembly"></param>
		/// <param name="relativePath"></param>
		/// <returns></returns>
		public static string GetAbsolutePath(Assembly assembly, string relativePath)
		{
			var assemblyFolderPath = new FileInfo(assembly.Location).Directory.FullName;
			return Path.GetFullPath(Path.Combine(assemblyFolderPath, relativePath));
		}
	}
}
