using System.Collections.Generic;
using System.Linq;

namespace linq_slideviews
{
	public class StatisticsTask
	{
		public static double GetMedianTimePerSlide(List<VisitRecord> visits, SlideType slideType)
		{
			try
			{
				var l = visits
					.OrderBy(x => x.DateTime)
					.GroupBy(x => x.UserId)
					.SelectMany(x => x.Bigrams())
					.Where(x=>x.Item1.SlideType == slideType)
					.Select(x => (x.Item2.DateTime - x.Item1.DateTime).TotalMinutes)
					.Where(x => x >= 1 && x <= 120)
					.Median();
				return l;
			}
			catch
			{
				return 0;
			}
		}
	}
}