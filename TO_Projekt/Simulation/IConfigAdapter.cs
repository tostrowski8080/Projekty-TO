using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TO_Projekt.Simulation;

namespace TO_Projekt.Adapter
{
    public interface IConfigAdapter
    {
        SimulationConfig GetConfig(int uiAntCount, double width, double height);
    }
}
