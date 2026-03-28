using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TO_Projekt.Observer
{
    public interface ISimulationObserver
    {
        void OnSimulationTick();
        void OnSimulationStateChanged(string status);
    }
}
