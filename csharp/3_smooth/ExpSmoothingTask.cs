using System.Collections.Generic;

namespace yield
{
	public static class ExpSmoothingTask
	{
		public static IEnumerable<DataPoint> SmoothExponentialy(this IEnumerable<DataPoint> data, double alpha)
		{
			var prevSmoothedY = 0.0;
			var isFirstIteration = true;
			foreach (var point in data)
			{
				if (isFirstIteration)
				{
					prevSmoothedY = point.OriginalY;
					isFirstIteration = false;
				}

				point.ExpSmoothedY = prevSmoothedY + alpha * (point.OriginalY - prevSmoothedY);
				yield return point;
				prevSmoothedY = point.ExpSmoothedY;
			}
		}
	}
}