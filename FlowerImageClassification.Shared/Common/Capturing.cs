using System;
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
			// Keep original standard output stream
			consoleWriter = Console.Out;
			fileWriter = new StreamWriter(pathToOutput);
			fileWriter.AutoFlush = true;
			// Redirect output stream
			Console.SetOut(fileWriter);
		}

		/// <summary>
		/// This function is called automatically when the object goes out of scope
		/// </summary>
		public void Dispose()
		{
			// Bring back the standard output stream
			Console.SetOut(consoleWriter);
			fileWriter.Close();
		}
	}
}
