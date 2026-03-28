using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using TO_Projekt.Simulation;

namespace TO_Projekt.Strategy
{
    public class ReturnHomeStrategy : IMovementStrategy
    {
        public Vector2D CalculateMove(AntContext ant, World world)
        {
            Vector2D dir = new Vector2D(world.Center.X - ant.X, world.Center.Y - ant.Y);
            if (dir.Length > 0) dir.Normalize();
            return dir;
        }
    }
}
