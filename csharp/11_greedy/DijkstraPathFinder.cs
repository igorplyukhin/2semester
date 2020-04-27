using System;
using System.Collections.Generic;
using System.Linq;
using Greedy.Architecture;
using System.Drawing;


namespace Greedy
{
    public class DijkstraPathFinder
    {
        private static readonly Point defaultPoint = new Point(int.MinValue, int.MinValue);

        private static readonly List<Size> moves = new List<Size>
        {
            new Size(1, 0), new Size(0, 1),
            new Size(-1, 0), new Size(0, -1)
        };

        public IEnumerable<PathWithCost> GetPathsByDijkstra(State state, Point start, IEnumerable<Point> targets)
        {
            var notVisitedChests = new HashSet<Point>(targets);
            var track = new Dictionary<Point, PathWithCost> {[start] = new PathWithCost(0, start)};
            var visitedPoints = new HashSet<Point>();
            while (notVisitedChests.Count > 0)
            {
                var (toOpen, bestPrice) = GetToOpenPointAndBestPrice(track, visitedPoints);

                if (toOpen == defaultPoint) yield break;
                if (notVisitedChests.Contains(toOpen))
                {
                    yield return track[toOpen];
                    notVisitedChests.Remove(toOpen);
                }

                ProceedNeighbours(state, track, toOpen, bestPrice);
                visitedPoints.Add(toOpen);
            }
        }

        private static Tuple<Point, int> GetToOpenPointAndBestPrice(Dictionary<Point, PathWithCost> track,
            HashSet<Point> visitedPoints)
        {
            var toOpen = defaultPoint;
            var bestPrice = int.MaxValue;
            foreach (var point in track.Keys)
            {
                if (visitedPoints.Contains(point) || track[point].Cost >= bestPrice) continue;
                bestPrice = track[point].Cost;
                toOpen = point;
            }

            return Tuple.Create(toOpen, bestPrice);
        }

        private void ProceedNeighbours(State state, Dictionary<Point, PathWithCost> track, Point toOpen, int bestPrice)
        {
            var possibleMoves = moves.Select(x => toOpen + x).Where(x => state.InsideMap(x) && !state.IsWallAt(x));
            foreach (var nextPoint in possibleMoves)
            {
                var currentPrice = track[toOpen].Cost + state.CellCost[nextPoint.X, nextPoint.Y];
                var path = new List<Point>(track[toOpen].Path) {nextPoint};
                if (!track.ContainsKey(nextPoint) || currentPrice < bestPrice)
                    track[nextPoint] = new PathWithCost(currentPrice, path.ToArray());
            }
        }
    }
}