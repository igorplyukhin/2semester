using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;

namespace Dungeon
{
    public class BfsTask
    {
        public static IEnumerable<SinglyLinkedList<Point>> FindPaths(Map map, Point start, Point[] chests)
        {
            var a = FindShortestPath(map, start, map.InitialPosition);
                if (a != null)
                    yield return a;
        }

        private static SinglyLinkedList<Point> FindShortestPath(Map map, Point start, Point finish)
        {
            var track = new Dictionary<Point, Point> {[start] = new Point(int.MinValue, int.MinValue)};
            var queue = new Queue<Point>();
            queue.Enqueue(start);
            var width = map.Dungeon.GetLength(0);
            var height = map.Dungeon.GetLength(1);

            while (queue.Count != 0)
            {
                var point = queue.Dequeue();
                

                for (var dy = -1; dy <= 1; dy++)
                for (var dx = -1; dx <= 1; dx++)
                {
                    var nextPoint = new Point(point.X + dx, point.Y + dy);
                    if (dx != 0 && dy != 0 || track.ContainsKey(nextPoint)) continue;
                    if (nextPoint.X < 0 || nextPoint.X >= width || nextPoint.Y < 0 || nextPoint.Y >= height) continue;
                    if (map.Dungeon[nextPoint.X, nextPoint.Y] != MapCell.Empty) continue;
                    track.Add(nextPoint, point);
                    queue.Enqueue(nextPoint);
                }

                if (track.ContainsKey(finish)) break;
            }

            if (!track.ContainsKey(finish))
                return null;
            var item = finish;
            var l = new SinglyLinkedList<Point>(item);
            item = track[item];

            while (item.X != int.MinValue && item.Y != int.MinValue)
            {
                l = new SinglyLinkedList<Point>(item, l);
                item = track[item];
            }
            
            return l;
        }
    }
}