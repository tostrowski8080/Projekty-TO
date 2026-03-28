using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using TO_Projekt.Simulation;

namespace TO_Projekt.Strategy
{
    public class FleeStrategy : IMovementStrategy
    {
        public Vector2D CalculateMove(AntContext ant, World world)
        {
            var threat = world.GetNearestThreat(ant.X, ant.Y);
            if (threat != null)
            {
                Vector2D dir = new Vector2D(ant.X - threat.X, ant.Y - threat.Y);
                if (dir.Length > 0) dir.Normalize();
                return dir;
            }
            return new Vector2D(0, 0);
        }
    }
}
