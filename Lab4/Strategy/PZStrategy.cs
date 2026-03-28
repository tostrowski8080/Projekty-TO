using Lab4.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab4.Strategy
{
    public class PZStrategy : IIncidentStrategy
    {
        public int RequiredTrucks => 3;
        public IncidentType Type => IncidentType.PZ;
        public string GetName() => "PZ";
    }
}
