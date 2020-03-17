using System;
using System.Collections.Generic;
using System.Drawing;

namespace func_rocket
{
    public class LevelsTask
    {
        private static readonly Vector initialPos = new Vector(200, 500);
        private static readonly Physics standardPhysics = new Physics();

        private static readonly Rocket defultRocket =
            new Rocket(initialPos, Vector.Zero, -0.5 * Math.PI);

        private static readonly Vector defaultTarget = new Vector(600, 200);

        public static IEnumerable<Level> CreateLevels() => levels;

        private static readonly List<Level> levels = new List<Level>
        {
            InitLevel("Zero"),
            InitLevel("Heavy", gravity: (size, l) => new Vector(0, 1) * 0.9),
            InitLevel("Up", target: new Vector(700, 500),
                gravity: (size, l) => new Vector(0, -1) * (300 / (size.Height - l.Y + 300.0))),
            InitLevel("WhiteHole", gravity: CalcWhiteHoleGravity),
            InitLevel("BlackHole", gravity: CalcBlackHoleGravity),
            InitLevel("BlackAndWhite",
                gravity: (size, l) => (CalcBlackHoleGravity(size, l) + CalcWhiteHoleGravity(size, l)) / 2)
        };

        private static Level InitLevel(string name = "NoName", Rocket rocket = null, Vector target = null,
            Gravity gravity = null, Physics physics = null)
        {
            rocket = rocket ?? defultRocket;
            target = target ?? defaultTarget;
            gravity = gravity ?? ((size, l) => Vector.Zero);
            physics = physics ?? standardPhysics;
            return new Level(name, rocket, target, gravity, physics);
        }

        private static Vector CalcWhiteHoleGravity(Size size, Vector l)
        {
            var d = l - defaultTarget;
            return d.Normalize() * 140 * d.Length / (d.Length * d.Length + 1);
        }

        private static Vector CalcBlackHoleGravity(Size size, Vector l)
        {
            var d = (defaultTarget + initialPos) / 2 - l;
            return d.Normalize() * 300 * d.Length / (d.Length * d.Length + 1);
        }
    }
}