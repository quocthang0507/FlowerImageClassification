using FlowerImageClassification.Shared.Common;
using FlowerImageClassification.Shared.ImageSchema;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace FlowerImageClassification.Shared.WebHelpers
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
			IEnumerable<string> imagesPath = Directory.
				GetFiles(pathToImageFolder, "*", SearchOption.AllDirectories).
				Where(file => Path.GetExtension(file).Contains("jpg", StringComparison.OrdinalIgnoreCase) || Path.GetExtension(file).Contains("png", StringComparison.OrdinalIgnoreCase));
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
			string assemblyFolderPath = new FileInfo(assembly.Location).Directory.FullName;
			return Path.GetFullPath(Path.Combine(assemblyFolderPath, relativePath));
		}

		public static void SplitDatasetToTrainTest(string pathToOriginalImageFolder, string pathToTrainingFolder, string pathToTestFolder, float testSize = 0.05f, bool random = true)
		{
			string[] subDirs = Directory.GetDirectories(pathToOriginalImageFolder);
			Console.WriteLine($"Found {subDirs.Length} sub-directories in original training folder");
			List<ImageData> trainingList = new List<ImageData>();
			List<ImageData> testList = new List<ImageData>();
			Console.WriteLine("I am splitting into two parts...");
			foreach (var subDir in subDirs)
			{
				IEnumerable<ImageData> list = LoadImagesFromDirectory(subDir);
				if (random)
					list = list.Shuffle();
				int testSampleSize = (int)(list.Count() * testSize);
				int trainingSampleSize = list.Count() - testSampleSize;
				for (int i = 0; i < list.Count(); i++)
				{
					if (i < trainingSampleSize)
						trainingList.Add(list.ElementAt(i));
					else
						testList.Add(list.ElementAt(i));
				}
			}
			Console.WriteLine($"There are {trainingList.Count + testList.Count} files, the training folder will have {trainingList.Count} files" +
				$" and the test folder will have {testList.Count} files");
			Console.WriteLine("I am copying original files into training folder...");
			foreach (var item in trainingList)
			{
				string fileName = Path.GetFileName(item.ImagePath);
				string folderPath = Path.Combine(pathToTrainingFolder, item.Label);
				Directory.CreateDirectory(folderPath);
				File.Copy(item.ImagePath, Path.Combine(folderPath, fileName), true);
			}
			Console.WriteLine("I am copying original files into test folder...");
			foreach (var item in testList)
			{
				string fileName = Path.GetFileName(item.ImagePath);
				string folderPath = Path.Combine(pathToTestFolder, item.Label);
				Directory.CreateDirectory(folderPath);
				File.Copy(item.ImagePath, Path.Combine(folderPath, fileName), true);
			}
		}
	}
}
