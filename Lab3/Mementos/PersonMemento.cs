using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab3.Mementos
{
    [Serializable]
    public class PersonMemento
    {
        public int Id { get; set; }
        public double PosX { get; set; }
        public double PosY { get; set; }
        public double VelX { get; set; }
        public double VelY { get; set; }

        public StateMemento StateData { get; set; }
    }
}
