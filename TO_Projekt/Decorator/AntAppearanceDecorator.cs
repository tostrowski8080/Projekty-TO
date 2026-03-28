using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace TO_Projekt.Decorator
{
    public abstract class AntAppearanceDecorator : IAntAppearance
    {
        protected IAntAppearance _wrappedAppearance;

        public AntAppearanceDecorator(IAntAppearance appearance)
        {
            _wrappedAppearance = appearance;
        }

        public virtual Brush GetColor() => _wrappedAppearance.GetColor();
        public virtual double GetSpeedModifier() => _wrappedAppearance.GetSpeedModifier();
    }
}
