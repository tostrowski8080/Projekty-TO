using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab3.Vector
{
    public interface IVector
    {
        double abs();
        double cdot(IVector param);
        double[] getComponents();
    }
}
