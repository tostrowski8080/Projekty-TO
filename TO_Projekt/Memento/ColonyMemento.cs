using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TO_Projekt.Simulation;

namespace TO_Projekt.Memento
{
    public class ColonyMemento
    {
        public int FoodStore { get; set; }
        public List<AntSnapshot> AntSnapshots { get; set; }
        public List<Point2D> FoodSnapshots { get; set; }
    }
}
