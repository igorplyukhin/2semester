using System;
using System.Collections.Generic;

namespace yield
{
    public static class MovingMaxTask
    {
        public static IEnumerable<DataPoint> MovingMax(this IEnumerable<DataPoint> data, int windowWidth)
        {
            var possibleMaximums = new LinkedList<double>();
            var currentWindowElements = new Queue<double>();
            foreach (var point in data)
            {
                var removedElement = 0.0;
                var firstElementIsRemoved = false;
                currentWindowElements.Enqueue(point.OriginalY);
                if (currentWindowElements.Count > windowWidth)
                {
                    removedElement = currentWindowElements.Dequeue();
                    firstElementIsRemoved = true;
                }

                if (firstElementIsRemoved && Math.Abs(removedElement - possibleMaximums.First.Value) < 1e-10)
                    possibleMaximums.RemoveFirst();

                while (possibleMaximums.Count > 0 && possibleMaximums.Last.Value <= point.OriginalY)
                    possibleMaximums.RemoveLast();

                possibleMaximums.AddLast(point.OriginalY);
                point.MaxY = possibleMaximums.First.Value;
                yield return point;
            }
        }
    }
}