using System;
using System.Drawing;
using System.Linq;

namespace func_rocket
{
	public class ForcesTask
	{
		public static RocketForce GetThrustForce(double forceValue) =>
			r => new Vector(1,0).Rotate(r.Direction) * Math.Abs(forceValue);

		public static RocketForce ConvertGravityToForce(Gravity gravity, Size spaceSize) =>
			r => gravity(spaceSize, r.Location);
		
		public static RocketForce Sum(params RocketForce[] forces) => 
			r => forces.Aggregate(Vector.Zero, (current, e) => current + e(r));
	}
}