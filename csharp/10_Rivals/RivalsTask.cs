using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using NUnit.Framework;

namespace Rivals
{
    public class RivalsTask
    {
        private static List<Size> directions = new List<Size>
            {new Size(1, 0),   new Size(0, 1), new Size(0, -1),new Size(-1, 0)};

        public static IEnumerable<OwnedLocation> AssignOwners(Map map)
        {
            var players =
                map.Players.ToDictionary(e => e, e => new Queue<Tuple<Point, int>>(new[] {Tuple.Create(e, 1)}));
            var playerNumber = -1;
            var visitedPoints = new HashSet<Point>();
            foreach (var e in AssignPlayersPositions(map, visitedPoints)) yield return e;

            while (players.Values.Any(x => x.Any()))
                foreach (var e in players.Values)
                {
                    playerNumber = ++playerNumber % map.Players.Length;
                    if (e.Count == 0) continue;
                    var (currentPoint, distance) = e.Dequeue();

                    foreach (var nextPoint in GetNeighbours(currentPoint))
                    {
                        if (IsWrongMove(currentPoint, nextPoint, visitedPoints, map)) continue;
                        visitedPoints.Add(nextPoint);
                        e.Enqueue(Tuple.Create(nextPoint, distance + 1));
                        yield return new OwnedLocation(playerNumber, nextPoint, distance);
                    }
                }
        }

        private static IEnumerable<Point> GetNeighbours(Point point) => directions.Select(e => point + e);

        private static List<OwnedLocation> AssignPlayersPositions(Map map, HashSet<Point> visitedPoints)
        {
            var i = 0;
            var playersPositions = new List<OwnedLocation>();
            foreach (var p in map.Players)
            {
                visitedPoints.Add(p);
                playersPositions.Add(new OwnedLocation(i++, p, 0));
            }

            return playersPositions;
        }

        private static bool IsWrongMove(Point oldP, Point newP, HashSet<Point> visitedPoints, Map map)
        {
            return Math.Abs(oldP.X - newP.X) != 0 && Math.Abs(oldP.Y - newP.Y) != 0
                   || visitedPoints.Contains(newP)
                   || !map.InBounds(newP)
                   || map.Maze[newP.X, newP.Y] != MapCell.Empty;
        }
    }
}