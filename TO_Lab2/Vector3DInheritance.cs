using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TO_Lab2
{
    public class Vector3DInheritance : Vector2D
    {
        private double z;

        public Vector3DInheritance(double x, double y, double z) : base(x, y)
        {
            this.z = z;
        }

        public override double abs()
        {
            return Math.Sqrt(x * x + y * y + z * z);
        }

        public override double cdot(IVector other)
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
            return x * otherComp[0] + y * otherComp[1] + z * oZ;
        }

        public override double[] getComponents()
        {
            return new double[] { x, y, z };
        }

        public Vector3DInheritance cross(IVector param)
        {
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

            double newX = y * oZ - z * otherComp[1];
            double newY = z * otherComp[0] - x * oZ;
            double newZ = x * otherComp[1] - y * otherComp[0];

            return new Vector3DInheritance(newX, newY, newZ);
        }

        public IVector getSrcV()
        {
            return new Vector2D(x, y);
        }
    }
}
