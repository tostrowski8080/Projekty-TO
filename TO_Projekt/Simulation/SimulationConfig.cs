using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TO_Projekt.Simulation
{
    public class SimulationConfig
    {
        public int InitialAnts { get; set; }
        public double MapWidth { get; set; }
        public double MapHeight { get; set; }
        public int SpawnCost { get; set; } = 2;
        public int FoodValue { get; set; } = 5;
    }
}
