using Lab4.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab4.Strategy
{
    public interface IIncidentStrategy
    {
        int RequiredTrucks { get; }
        IncidentType Type { get; }
        string GetName();
    }
}
