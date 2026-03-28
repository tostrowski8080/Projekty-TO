using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace TO_Lab2
{
    public class Vector2D : IVector
    {
        protected double x;
        protected double y;

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
            if (oComp.Length < 2) {
                throw new ArgumentException("Error: Not enough components");
            }
            return x * oComp[0] + y * oComp[1];
        }
    }
}
    