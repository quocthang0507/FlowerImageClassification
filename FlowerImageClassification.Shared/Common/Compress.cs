﻿using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.GZip;
using ICSharpCode.SharpZipLib.Tar;
using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FlowerImageClassification.Shared.Common
{
	/// <summary>
	/// This class is based on DeepLearning_ImageClassification_Training
	/// </summary>
	public class Compress
	{
		public static void ExtractGZip(string gzipFileName, string targetDir)
		{
			// Use a 4K buffer. Any larger is a waste.    
			byte[] dataBuffer = new byte[4096];

			using (System.IO.Stream fs = new FileStream(gzipFileName, FileMode.Open, FileAccess.Read))
			{
				using (GZipInputStream gzipStream = new GZipInputStream(fs))
				{
					// Change this to your needs
					string fnOut = Path.Combine(targetDir, Path.GetFileNameWithoutExtension(gzipFileName));

					using (FileStream fsOut = File.Create(fnOut))
					{
						StreamUtils.Copy(gzipStream, fsOut, dataBuffer);
					}
				}
			}
		}

		public static void UnZip(String gzArchiveName, String destFolder)
		{
			string flag = gzArchiveName.Split(Path.DirectorySeparatorChar).Last().Split('.').First() + ".bin";
			if (File.Exists(Path.Combine(destFolder, flag))) return;

			Console.WriteLine($"Extracting.");
			Task task = Task.Run(() =>
			{
				ZipFile.ExtractToDirectory(gzArchiveName, destFolder);
			});

			while (!task.IsCompleted)
			{
				Thread.Sleep(200);
				Console.Write(".");
			}

			File.Create(Path.Combine(destFolder, flag));
			Console.WriteLine("");
			Console.WriteLine("Extracting is completed.");
		}

		public static void ExtractTGZ(String gzArchiveName, String destFolder)
		{
			string flag = gzArchiveName.Split(Path.DirectorySeparatorChar).Last().Split('.').First() + ".bin";
			if (File.Exists(Path.Combine(destFolder, flag))) return;

			Console.WriteLine($"Extracting.");
			Task task = Task.Run(() =>
			{
				using FileStream inStream = File.OpenRead(gzArchiveName);
				using GZipInputStream gzipStream = new GZipInputStream(inStream);
				using TarArchive tarArchive = TarArchive.CreateInputTarArchive(gzipStream, Encoding.UTF8);
				tarArchive.ExtractContents(destFolder);
			});

			while (!task.IsCompleted)
			{
				Thread.Sleep(200);
				Console.Write(".");
			}

			File.Create(Path.Combine(destFolder, flag));
			Console.WriteLine("");
			Console.WriteLine("Extracting is completed.");
		}
	}
}
