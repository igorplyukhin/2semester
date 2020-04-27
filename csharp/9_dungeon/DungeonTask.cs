using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Dungeon
{
    public class DungeonTask
    {
        public static MoveDirection[] FindShortestPath(Map map)
        {
            var straightWay = GetStraightWay(map).ToList();
            var straightDirections = ConvertPointsToDirections(straightWay).ToArray();
            if (straightWay.Any(point => map.Chests.Contains(point)))
                return straightDirections;

            var pathsToPlayer = BfsTask.FindPaths(map, map.InitialPosition, map.Chests).ToList();
            if (!pathsToPlayer.Any())
                return straightDirections;
            var pathsToExit = BfsTask.FindPaths(map, map.Exit, map.Chests);
            var fullPaths = pathsToPlayer.Join(pathsToExit, list => list.Value, list => list.Value,
                (l1, l2) => l1
                    .Reverse()
                    .Concat(l2.Skip(1))
                    .ToList()).ToList();
            if (!fullPaths.Any())
                return new MoveDirection[0];

            var shortestPath = fullPaths.Aggregate((min, x) => x.Count < min.Count ? x : min);

            return ConvertPointsToDirections(shortestPath).ToArray();
        }

        private static IEnumerable<MoveDirection> ConvertPointsToDirections(List<Point> points)
        {
            return points.Zip(points.Skip(1), (p1, p2) =>
                Walker.ConvertOffsetToDirection(new Size(p2.X - p1.X, p2.Y - p1.Y)));
        }

        private static IEnumerable<Point> GetStraightWay(Map map)
        {
            var path =
                BfsTask.FindPaths(map, map.InitialPosition, new[] {new Point(map.Exit.X, map.Exit.Y)}).ToList();
            return path.Any()
                ? path[0].Reverse()
                : new List<Point>();
        }
    }
}