using System.Collections.Generic;

namespace yield
{
    public static class ExpSmoothingTask
    {
        public static IEnumerable<DataPoint> SmoothExponentialy(this IEnumerable<DataPoint> data, double alpha)
        {
            var prevSmoothedY = double.NaN;
            foreach (var point in data)
            {
                prevSmoothedY = double.IsNaN(prevSmoothedY)
                    ? point.OriginalY
                    : prevSmoothedY + alpha * (point.OriginalY - prevSmoothedY);
                point.ExpSmoothedY = prevSmoothedY;
                yield return point;
            }
        }
    }
}