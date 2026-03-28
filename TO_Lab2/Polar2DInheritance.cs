using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;

namespace TO_Lab2
{
    public class Polar2DInheritance : Vector2D
    {
        public Polar2DInheritance(double x, double y) : base(x, y) { }

        public double getAngle()
        {
            return Math.Atan2(y, x);
        }
    }
}
