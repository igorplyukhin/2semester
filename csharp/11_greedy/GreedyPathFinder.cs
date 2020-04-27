using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Greedy.Architecture;
using Greedy.Architecture.Drawing;

namespace Greedy
{
	public class GreedyPathFinder : IPathFinder
	{
		public List<Point> FindPathToCompleteGoal(State state)
		{
			if (state.Chests.Count < state.Goal) 
				return new List<Point>();
			var path = new List<Point>();
			var start = state.Position;
			var chests = new HashSet<Point>(state.Chests);
			var finder = new DijkstraPathFinder();

			for (var i = 0; i < state.Goal; i++)
			{
				var shortestPath = finder.GetPathsByDijkstra(state, start, chests).FirstOrDefault();
				if (shortestPath == null) 
					return new List<Point>();
				if (state.Energy < shortestPath.Cost) 
					return new List<Point>();
				path.AddRange(shortestPath.Path.Skip(1));
				chests.Remove(shortestPath.End);
				start = shortestPath.End;
			}

			return path;
		}
	}
}