using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using TO_Projekt.Simulation;

namespace TO_Projekt.State
{
    public enum AntActionState { Foraging, Returning, Fleeing }
    public abstract class AntState
    {
        protected AntContext _ant;
        public void SetContext(AntContext ant) => _ant = ant;
        public abstract void Update(World world);
    }
}
