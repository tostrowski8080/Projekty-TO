using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace TO_Projekt.Decorator
{
    public class CarrierAppearanceDecorator : AntAppearanceDecorator
    {
        public CarrierAppearanceDecorator(IAntAppearance appearance) : base(appearance) { }

        public override Brush GetColor() => Brushes.Green;
        public override double GetSpeedModifier() => 0.5;
    }
}
