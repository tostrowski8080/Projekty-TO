using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TO_Projekt.Simulation;

namespace TO_Projekt.Adapter
{
    public class UIConfigAdapter : IConfigAdapter
    {
        public SimulationConfig GetConfig(int uiAntCount, double width, double height)
        {
            return new SimulationConfig
            {
                InitialAnts = uiAntCount,
                MapWidth = width,
                MapHeight = height,
                SpawnCost = 2,
                FoodValue = 5
            };
        }
    }
}
