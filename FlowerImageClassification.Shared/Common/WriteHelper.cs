using System;

namespace FlowerImageClassification.Shared.Common
{
	/// <summary>
	/// The Console.Write methods with color
	/// </summary>
	public class WriteHelper
	{
		public static void Print_WarningText(string text)
		{
			Console.ForegroundColor = ConsoleColor.Yellow;
			Console.WriteLine(text);
			Console.ResetColor();
		}

		public static void Print_SuccessText(string text)
		{
			Console.ForegroundColor = ConsoleColor.Green;
			Console.WriteLine(text);
			Console.ResetColor();
		}

		public static void Print_CenteredTitle(string title, int maxWidth)
		{
			int len = title.Length;
			if (maxWidth - len > 0)
			{
				int half = (maxWidth - len) / 2;
				Console.WriteLine(new string(' ', half) + title);
			}
		}
	}
}
