using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace TO_Projekt.Decorator
{
    public class BasicAntAppearance : IAntAppearance
    {
        public Brush GetColor() => Brushes.Black;
        public double GetSpeedModifier() => 1.0;
    }
}
