using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TO_Projekt.Simulation
{
    public class Point2D
    {
        public double X { get; set; }
        public double Y { get; set; }
        public Point2D(double x, double y)
        {
            X = x;
            Y = y;
        }

        public static Vector2D operator -(Point2D p1, Point2D p2) => new Vector2D(p1.X - p2.X, p1.Y - p2.Y);
    }
}
