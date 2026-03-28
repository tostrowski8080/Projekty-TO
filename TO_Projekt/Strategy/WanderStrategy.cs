using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using TO_Projekt.Simulation;

namespace TO_Projekt.Strategy
{
    public class WanderStrategy : IMovementStrategy
    {
        private static Random _rng = new Random();

        public Vector2D CalculateMove(AntContext ant, World world)
        {
            Vector2D pheromoneAttraction = new Vector2D(0, 0);
            int detected = 0;

            if (world.Pheromones.Count > 0)
            {
                double sensorRadiusSq = 40 * 40;

                foreach (var p in world.Pheromones)
                {
                    double distSq = Math.Pow(p.X - ant.X, 2) + Math.Pow(p.Y - ant.Y, 2);

                    if (distSq < sensorRadiusSq)
                    {
                        Vector2D dirToPheromone = new Point2D(p.X, p.Y) - new Point2D(ant.X, ant.Y);

                        double dotProduct = ant.Direction.X * dirToPheromone.X + ant.Direction.Y * dirToPheromone.Y;

                        if (dotProduct > 0)
                        {
                            pheromoneAttraction = pheromoneAttraction + dirToPheromone;
                            detected++;
                        }
                    }
                }
            }

            Vector2D newDir;

            if (detected > 0)
            {
                pheromoneAttraction.Normalize();

                newDir = (pheromoneAttraction * 0.6) + (ant.Direction * 0.4);

                double noise = (_rng.NextDouble() * 0.4) - 0.2;
                double angle = Math.Atan2(newDir.Y, newDir.X) + noise;
                newDir = new Vector2D(Math.Cos(angle), Math.Sin(angle));
            }
            else
            {
                double currentAngle = Math.Atan2(ant.Direction.Y, ant.Direction.X);
                double angleChange = (_rng.NextDouble() * 0.6) - 0.3;
                double newAngle = currentAngle + angleChange;
                newDir = new Vector2D(Math.Cos(newAngle), Math.Sin(newAngle));
            }

            newDir.Normalize();
            ant.Direction = newDir;
            return newDir;
        }
    }
}
