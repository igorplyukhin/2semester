using System;

namespace func_rocket
{
	public class ControlTask
	{
		private const double AngleDelta = 0.00001;
		private const double SpeedLimit = 2.25;

		private static bool IsTooFast(Rocket rocket) => Math.Abs(rocket.Velocity.Angle - rocket.Direction) > SpeedLimit;
		
		public static Turn ControlRocket(Rocket rocket, Vector target)
		{
			var currentAngle = (rocket.Velocity.Angle + rocket.Direction) / 2;
			var appropriateAngle = (target - rocket.Location).Angle;
			if (Math.Abs(currentAngle - appropriateAngle) < AngleDelta)
				return Turn.None;
			if (IsTooFast(rocket))
				return rocket.Direction < appropriateAngle ? Turn.Right : Turn.Left;
			return currentAngle < appropriateAngle ? Turn.Right : Turn.Left;
		}
	}
}