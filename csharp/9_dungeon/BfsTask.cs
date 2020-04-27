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
            
            while (queue.Count != 0)
            {
                var point = queue.Dequeue();
                foreach (var delta in Walker.PossibleDirections)
                {
                    var walker = new Walker(point.Value).WalkInDirection(map, Walker.ConvertOffsetToDirection(delta));
                    if (walker.PointOfCollision != null || visitedPoints.Contains(walker.Position)) continue;
                    var nextPoint = walker.Position;
                    visitedPoints.Add(nextPoint);
                    var nextNode = new SinglyLinkedList<Point>(nextPoint, point);
                    if (hashedChests.Contains(nextPoint))
                        yield return nextNode;
                    queue.Enqueue(nextNode);
                }
            }
        }
    }
}