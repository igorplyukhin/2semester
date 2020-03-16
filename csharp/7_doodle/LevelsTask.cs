using System;
using System.Collections.Generic;
using System.Drawing;

namespace func_rocket
{
    public class LevelsTask
    {
        private static Vector initialPos = new Vector(200, 500);
        static readonly Physics standardPhysics = new Physics();

        private static Rocket rocket =
            new Rocket(initialPos, Vector.Zero, -0.5 * Math.PI);

        private static Vector target = new Vector(600, 200);

        public class DefaultLevel : Level
        {
            public DefaultLevel(string name = null, Rocket rocket = null, Vector target = null,
                Gravity gravity = null, Physics physics = null) : 
                base("no name", LevelsTask.rocket, LevelsTask.target, (size, l) => Vector.Zero, standardPhysics)
            {
                if (name != null)
                    Level.Name = name;
                if (rocket != null)
                    rocket = LevelsTask.rocket;
                if (target != null)
                    target = LevelsTask.target;
                if (gravity != null)
                    gravity = (size, l) => Vector.Zero;
                if (physics != null)
                    physics = standardPhysics;
            } 
        }

        private static List<Level> levels = new List<Level>
        {
            new Level("Zero", rocket, target, (size, l) => Vector.Zero, standardPhysics),
            new Level("Heavy", rocket, target,
                (size, l) => new Vector(0, 1) * 0.9, standardPhysics),
            new Level("Up", rocket, new Vector(700, 500),
                (size, l) => new Vector(0, -1) * (300 / (size.Height - l.Y + 300.0)), standardPhysics),
            new Level("WhiteHole", rocket, target, CalcWhiteHoleGravity, standardPhysics),
            new Level("BlackHole", rocket, target, CalcBlackHoleGravity, standardPhysics),
            new Level("BlackAndWhite", rocket, target,
                (size, l) => (CalcBlackHoleGravity(size, l) + CalcWhiteHoleGravity(size, l)) / 2,
                standardPhysics)
        };

        public static IEnumerable<Level> CreateLevels()
        {
            
        };

        private static Vector CalcWhiteHoleGravity(Size size, Vector l)
        {
            var d = l - target;
            return d.Normalize() * 140 * d.Length / (d.Length * d.Length + 1);
        }

        private static Vector CalcBlackHoleGravity(Size size, Vector l)
        {
            var blackHolePos = (target + initialPos) / 2;
            var d = blackHolePos - l;
            return d.Normalize() * 300 * d.Length / (d.Length * d.Length + 1);
        }
    }
}