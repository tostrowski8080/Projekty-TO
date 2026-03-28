using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TO_Lab2
{
    public class Polar2DAdapter : IPolar2D, IVector
    {
        private Vector2D srcVector;
        public Polar2DAdapter(Vector2D srcVector)
        {
            this.srcVector = srcVector;
        }
        public double abs()
        {
            return srcVector.abs();
        }

        public double cdot(IVector other)
        {
            return srcVector.cdot(other);
        }

        public double[] getComponents()
        {
            return srcVector.getComponents();
        }

        public double getAngle()
        {
            double[] srcComp = srcVector.getComponents();
            if (srcComp.Length < 2)
            {
                throw new ArgumentException("Error: Not enough components");
            }
            return Math.Atan2(srcComp[1], srcComp[0]);
        }
    }
}
