using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Dungeon
{
    public class DungeonTask
    {
        public static MoveDirection[] FindShortestPath(Map map)
        {
            var straightWay = GetStraightWay(map);
            var straightDirections = ConvertPointsToDirections(straightWay).ToArray();
            foreach (var point in straightWay)
            {
                if (map.Chests.Contains(point))
                    return straightDirections;
            }
            
            var pathsToPlayer = BfsTask.FindPaths(map, map.InitialPosition, map.Chests);
            if (!pathsToPlayer.Any())
                return straightDirections;
            var pathsToExit = BfsTask.FindPaths(map, map.Exit, map.Chests);
            var fullPaths = pathsToPlayer.Join(pathsToExit, list => list.Value, list => list.Value,
                (l1, l2) => l1
                    .Reverse()
                    .Concat(l2.Skip(1))
                    .ToList());
            if (!fullPaths.Any())
                return new MoveDirection[0];
            
            var shortestPath = fullPaths.Aggregate((min, x) => x.Count < min.Count ? x : min);
            
            return ConvertPointsToDirections(shortestPath).ToArray();
        }

        private static IEnumerable<MoveDirection> ConvertPointsToDirections(IEnumerable<Point> points)
        {
            return points.Zip(points.Skip(1), (p1, p2) =>
            {
                var dx = p2.X - p1.X;
                var dy = p2.Y - p1.Y;
                if (dx < 0)
                    return MoveDirection.Left;
                if (dx > 0)
                    return MoveDirection.Right;
                if (dy < 0)
                    return MoveDirection.Up;
                return MoveDirection.Down;
            }).ToArray();
        }

        private static IEnumerable<Point> GetStraightWay(Map map)
        {
            var path = 
                BfsTask.FindPaths(map, map.InitialPosition, new[] {new Point(map.Exit.X, map.Exit.Y)});
            return path;
        }
    }
}