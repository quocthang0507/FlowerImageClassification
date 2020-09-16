using System;
using System.IO;
using System.Text;

namespace FlowerImageClassification.Shared.Common
{
	/// <summary>
	/// Mirror the standard output stream console
	/// Reference from: https://stackoverflow.com/a/6927051
	/// </summary>
	public class MirrorOutput : IDisposable
	{
		private StreamWriter fileWriter;
		private TextWriter doubleWriter;
		private TextWriter stdOutput;

		public class DoubleWriter : TextWriter
		{
			private TextWriter writer1;
			private TextWriter writer2;

			public DoubleWriter(TextWriter writer1, TextWriter writer2)
			{
				this.writer1 = writer1;
				this.writer2 = writer2;
			}

			public override Encoding Encoding => writer1.Encoding;

			public override void Flush()
			{
				writer1.Flush();
				writer2.Flush();
			}

			public override void Write(char value)
			{
				writer1.Write(value);
				writer2.Write(value);
			}
		}

		public MirrorOutput(string pathToOutput)
		{
			stdOutput = Console.Out;
			Console.OutputEncoding = Encoding.UTF8;
			try
			{
				fileWriter = new StreamWriter(pathToOutput);
				fileWriter.AutoFlush = true;
				doubleWriter = new DoubleWriter(fileWriter, stdOutput);
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
				return;
			}
			Console.SetOut(doubleWriter);
		}

		public void Dispose()
		{
			// Bring back the standard output stream
			Console.SetOut(stdOutput);
			fileWriter.Flush();
			fileWriter.Close();
		}
	}
}
