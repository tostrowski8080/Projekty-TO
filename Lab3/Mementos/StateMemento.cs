using System;
using Lab3.States;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab3.Mementos
{
    [Serializable]
    public class StateMemento
    {
        public HealthStatus Status { get; set; }
        public double TimeRemaining { get; set; }
        public Dictionary<int, double> ExposureTimes { get; set; }
    }
}
