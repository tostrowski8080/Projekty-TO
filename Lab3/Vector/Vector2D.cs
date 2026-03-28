using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab3.Vector
{
    public class Vector2D : IVector
    {
        protected double x;
        protected double y;

        public Vector2D() 
        {
            this.x = 0;
            this.y = 0;
        }

        public Vector2D(double x, double y)
        {
            this.x = x;
            this.y = y;
        }

        public virtual double[] getComponents()
        {
            return new double[] { x, y };
        }

        public virtual double abs()
        {
            return Math.Sqrt(x * x + y * y);
        }

        public virtual double cdot(IVector other)
        {
            double[] oComp = other.getComponents();
            if (oComp.Length < 2)
            {
                throw new ArgumentException("Error: Not enough components");
            }
            return x * oComp[0] + y * oComp[1];
        }

        public double X { get => x; set => x = value; }
        public double Y { get => y; set => y = value; }

        public static Vector2D operator +(Vector2D a, Vector2D b)
        {
            return new Vector2D(a.x + b.x, a.y + b.y);
        }

        public static Vector2D operator -(Vector2D a, Vector2D b)
        {
            return new Vector2D(a.x - b.x, a.y - b.y);
        }

        public static Vector2D operator *(Vector2D a, double scalar)
        {
            return new Vector2D(a.x * scalar, a.y * scalar);
        }

        public static double Distance(Vector2D a, Vector2D b)
        {
            double dx = a.x - b.x;
            double dy = a.y - b.y;
            return Math.Sqrt(dx * dx + dy * dy);
        }

        public Vector2D Normalize()
        {
            double length = this.abs();
            if (length == 0) return new Vector2D(0, 0);
            return new Vector2D(x / length, y / length);
        }
    }
}
