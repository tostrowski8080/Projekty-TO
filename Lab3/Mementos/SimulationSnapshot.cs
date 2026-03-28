using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab3.Mementos
{
    [Serializable]
    public class SimulationSnapshot
    {
        public double CurrentTime { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
        public List<PersonMemento> Individuals { get; set; }
    }
}
