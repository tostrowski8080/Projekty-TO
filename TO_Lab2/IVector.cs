using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TO_Lab2
{
    public interface IVector
    {
        double abs();
        double cdot(IVector other);
        double[] getComponents();
    }
}
