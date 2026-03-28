using Lab4.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab4.Strategy
{
    public class MZStrategy : IIncidentStrategy
    {
        public int RequiredTrucks => 2;
        public IncidentType Type => IncidentType.MZ;
        public string GetName() => "MZ";
    }
}
