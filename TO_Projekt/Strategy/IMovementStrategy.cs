using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using TO_Projekt.Simulation;

namespace TO_Projekt.Strategy
{
    public interface IMovementStrategy
    {
        Vector2D CalculateMove(AntContext ant, World world);
    }
}
