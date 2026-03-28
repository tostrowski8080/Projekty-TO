using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab4.Vector
{
    public class Vector2D
    {
        public double X { get; set; }
        public double Y { get; set; }

        public Vector2D(double x, double y)
        {
            X = x;
            Y = y;
        }

        public static double Distance(Vector2D a, Vector2D b)
        {
            double dx = a.X - b.X;
            double dy = a.Y - b.Y;
            return Math.Sqrt(dx * dx + dy * dy);
        }

        public static Vector2D operator +(Vector2D a, Vector2D b) => new Vector2D(a.X + b.X, a.Y + b.Y);
        public static Vector2D operator -(Vector2D a, Vector2D b) => new Vector2D(a.X - b.X, a.Y - b.Y);
        public static Vector2D operator *(Vector2D a, double scalar) => new Vector2D(a.X * scalar, a.Y * scalar);

        public double Length() => Math.Sqrt(X * X + Y * Y);

        public Vector2D Normalize()
        {
            double len = Length();
            if (len == 0) return new Vector2D(0, 0);
            return new Vector2D(X / len, Y / len);
        }
    }
}
