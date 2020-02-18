using System.Collections.Generic;

namespace yield
{
    public static class MovingMaxTask
    {
        public static IEnumerable<DataPoint> MovingMax(this IEnumerable<DataPoint> data, int windowWidth)
        {
            var possibleMaximums = new LinkedList<double>();
            foreach (var point in data)
            {
                while (possibleMaximums.Count > 0 && possibleMaximums.Last.Value <= point.OriginalY)
                {
                    possibleMaximums.RemoveLast();
                }

                possibleMaximums.AddLast(point.OriginalY);
                if (possibleMaximums.Count > windowWidth)
                    possibleMaximums.RemoveFirst();
                
                point.MaxY = possibleMaximums.First.Value;
                yield return point;
            }
        }
    }
}