using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TO_Lab2
{
    public class Vector3DDecorator : IVector
    {
        private IVector srcVector;
        private double z;

        public Vector3DDecorator(IVector srcVector, double z)
        {
            this.srcVector = srcVector;
            this.z = z;
        }
        public double abs()
        {
            double[] srcComp = srcVector.getComponents();
            if (srcComp.Length < 2)
            {
                throw new ArgumentException("Error: Not enough components");
            }
            return Math.Sqrt(srcComp[0] * srcComp[0] + srcComp[1] * srcComp[1] + z * z);
        }

        public double cdot(IVector other)
        {
            double[] otherComp = other.getComponents();
            double oZ;
            if (otherComp.Length < 3)
            {
                if (otherComp.Length < 2)
                {
                    throw new ArgumentException("Error: Not enough components");
                }
                oZ = 0;
            }
            else oZ = otherComp[2];
            double[] srcComp = srcVector.getComponents();
            if (srcComp.Length < 2)
            {
                throw new ArgumentException("Error: Not enough components");
            }
            return otherComp[0] * srcComp[0] + otherComp[1] * srcComp[1] + oZ * z;
        }

        public double[] getComponents()
        {
            double[] srcComp = srcVector.getComponents();
            if (srcComp.Length < 2)
            {
                throw new ArgumentException("Error: Not enough components");
            }
            return new double[] { srcComp[0], srcComp[1], z };
        }

        public Vector3DDecorator cross(IVector param)
        {
            double[] srcComp = srcVector.getComponents();
            if (srcComp.Length < 2)
            {
                throw new ArgumentException("Error: Not enough components");
            }
            double[] otherComp = param.getComponents();
            double oZ;
            if (otherComp.Length < 3)
            {
                if (otherComp.Length < 2)
                {
                    throw new ArgumentException("Error: Not enough components");
                }
                oZ = 0;
            }
            else oZ = otherComp[2];

                double newX = srcComp[1] * oZ - z * otherComp[1];
            double newY = z * otherComp[0] - srcComp[0] * oZ;
            double newZ = srcComp[0] * otherComp[1] - srcComp[1] * otherComp[0];

            return new Vector3DDecorator(new Vector2D(newX, newY), newZ);
        }

        public IVector getSrcV()
        {
            return srcVector;
        }
    }
}
