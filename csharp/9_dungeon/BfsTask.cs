using System;
using System.Collections.Generic;
using System.Drawing;

namespace Dungeon
{
    public class BfsTask
    {
        public static IEnumerable<SinglyLinkedList<Point>> FindPaths(Map map, Point start, Point[] chests)
        {
            var visitedPoints = new HashSet<Point>();
            var hashedChests = new HashSet<Point>(chests);
            var queue = new Queue<SinglyLinkedList<Point>>();
            queue.Enqueue(new SinglyLinkedList<Point>(start));
            var width = map.Dungeon.GetLength(0);
            var height = map.Dungeon.GetLength(1);

            while (queue.Count != 0)
            {
                var point = queue.Dequeue();
                for (var dy = -1; dy <= 1; dy++)
                for (var dx = -1; dx <= 1; dx++)
                {
                    var nextPoint = new Point(point.Value.X + dx, point.Value.Y + dy);
                    if (IsWrongMove(point.Value, nextPoint, width, height, visitedPoints, map)) continue;
                    visitedPoints.Add(nextPoint);
                    var nextNode = new SinglyLinkedList<Point>(nextPoint, point);
                    if (hashedChests.Contains(nextPoint))
                        yield return nextNode;
                    queue.Enqueue(nextNode);
                }
            }
        }

        private static bool IsWrongMove(Point oldP, Point newP, int width, int height, 
            HashSet<Point> visitedPoints, Map map)
        {
            return Math.Abs(oldP.X - newP.X) != 0 && Math.Abs(oldP.Y - newP.Y) != 0
                   || visitedPoints.Contains(newP)
                   || newP.X < 0 
                   || newP.X >= width 
                   || newP.Y < 0 
                   || newP.Y >= height 
                   || map.Dungeon[newP.X, newP.Y] != MapCell.Empty;
        }
    }
}