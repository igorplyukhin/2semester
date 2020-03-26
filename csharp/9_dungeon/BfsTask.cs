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
            return chests.Select(e => FindShortestPath(map, e, start));
        }

        private static SinglyLinkedList<Point> FindShortestPath(Map map, Point start, Point finish)
        {
            var track = new Dictionary<Point, Point> {[start] = Point.Empty};
            var queue = new Queue<Point>();
            queue.Enqueue(start);
            var width = map.Dungeon.GetLength(0);
            var height = map.Dungeon.GetLength(1);

            while (queue.Count != 0)
            {
                var point = queue.Dequeue();
                if (point.X < 0 || point.X >= width || point.Y < 0 || point.Y >= height) continue;
                if (map.Dungeon[point.X, point.Y] != MapCell.Empty) continue;

                for (var dy = -1; dy <= 1; dy++)
                for (var dx = -1; dx <= 1; dx++)
                {
                    var nextPoint = new Point(point.X + dx, point.Y + dy);
                    if (dx != 0 && dy != 0 ||  track.ContainsKey(nextPoint)) continue;
                    track[nextPoint] = point;
                    queue.Enqueue(new Point {X = point.X + dx, Y = point.Y + dy});
                }
                
                if (track.ContainsKey(finish)) break;
            }

            var item = finish;
            var l = new SinglyLinkedList<Point>(item);
            while (item != null)
            {
                l = new SinglyLinkedList<Point>(track[item], l);
            }

            return l;
        }
    }
}