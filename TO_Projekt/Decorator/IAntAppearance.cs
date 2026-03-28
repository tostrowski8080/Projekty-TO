using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace TO_Projekt.Decorator
{
    public interface IAntAppearance
    {
        Brush GetColor();
        double GetSpeedModifier();
    }
}
