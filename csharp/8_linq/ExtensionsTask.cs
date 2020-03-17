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

		/// <returns>
		/// Возвращает последовательность, состоящую из пар соседних элементов.
		/// Например, по последовательности {1,2,3} метод должен вернуть две пары: (1,2) и (2,3).
		/// </returns>
		public static IEnumerable<Tuple<T, T>> Bigrams<T>(this IEnumerable<T> items)
		{
			var l = new List<Tuple<T,T>>();
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
			}
		}
	}
}