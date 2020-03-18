using System;
using System.Collections.Generic;
using System.Linq;

namespace linq_slideviews
{
	public static class ExtensionsTask
	{
		public static double Median(this IEnumerable<double> items)
		{
			var sortedItems = items.OrderBy(x => x).ToList();
			if (sortedItems.Count == 0)
				throw new InvalidOperationException();
			if (sortedItems.Count % 2 == 0)
				return (sortedItems[sortedItems.Count / 2] + sortedItems[sortedItems.Count / 2 - 1]) / 2; 
			return sortedItems[sortedItems.Count / 2];
		}

		public static IEnumerable<Tuple<T, T>> Bigrams<T>(this IEnumerable<T> items)
		{
			var prev = default(T);
			var isFirstIteration = true;
			foreach (var e in items)
			{
				if (isFirstIteration)
				{
					prev = e;
					isFirstIteration = false;
					continue;
				}
				yield return Tuple.Create(prev, e);
				prev = e;
			}
		}
	}
}