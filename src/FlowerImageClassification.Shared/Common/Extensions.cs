using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FlowerImageClassification.Shared.Common
{
	/// <summary>
	/// Reference from https://stackoverflow.com/a/5807238
	/// </summary>
	public static class EnumerableExtensions
	{
		public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source)
		{
			return source.Shuffle(new Random());
		}

		public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source, Random rand)
		{
			if (source == null || rand == null) throw new ArgumentNullException();

			return source.ShuffleIterator(rand);
		}

		private static IEnumerable<T> ShuffleIterator<T>(
			this IEnumerable<T> source, Random rng)
		{
			var buffer = source.ToList();
			for (int i = 0; i < buffer.Count; i++)
			{
				int j = rng.Next(i, buffer.Count);
				yield return buffer[j];

				buffer[j] = buffer[i];
			}
		}
	}
}
