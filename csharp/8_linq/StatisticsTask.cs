using System;
using System.Collections.Generic;
using System.Linq;

namespace linq_slideviews
{
	public class StatisticsTask
	{
		public static double GetMedianTimePerSlide(List<VisitRecord> visits, SlideType slideType)
		{
			var l = visits
				.OrderBy(x => x.DateTime)
				.GroupBy(x => x.UserId);

			var r = 0;
			return r;
		}
	}
}