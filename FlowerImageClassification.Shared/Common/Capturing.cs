﻿using System;
using System.IO;

namespace FlowerImageClassification.Shared.Common
{
	/// <summary>
	/// Redirect Console Application output to file
	/// </summary>
	public class Capturing : IDisposable
	{
		private StreamWriter fileWriter;
		private TextWriter consoleWriter;

		/// <summary>
		/// Initialize class and indicate the path to save the output file
		/// </summary>
		/// <param name="pathToOutput"></param>
		public Capturing(string pathToOutput)
		{
			consoleWriter = Console.Out;
			fileWriter = new StreamWriter(pathToOutput);
			fileWriter.AutoFlush = true;
			Console.SetOut(fileWriter);
		}

		/// <summary>
		/// This function is called automatically when the object goes out of scope
		/// </summary>
		public void Dispose()
		{
			Console.SetOut(consoleWriter);
			fileWriter.Close();
		}
	}
}
