using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using TO_Projekt.Iterator;
using TO_Projekt.Memento;
using TO_Projekt.Models;
using TO_Projekt.Observer;

namespace TO_Projekt.Simulation
{
    public class World : ISimulationSubject
    {
        private List<ISimulationObserver> _observers = new List<ISimulationObserver>();
        public List<AntContext> Ants { get; private set; } = new List<AntContext>();
        public List<EntityViewModel> Food { get; private set; } = new List<EntityViewModel>();
        public List<ObstacleViewModel> Obstacles { get; private set; } = new List<ObstacleViewModel>();
        public List<EntityViewModel> Threats { get; private set; } = new List<EntityViewModel>();
        public List<PheromoneViewModel> Pheromones { get; private set; } = new List<PheromoneViewModel>();

        private Dictionary<EntityViewModel, Vector2D> _threatVelocities = new Dictionary<EntityViewModel, Vector2D>();

        public Point2D Center { get; private set; }
        public int FoodStore { get; private set; }
        public double Width { get; }
        public double Height { get; }
        private SimulationConfig _config;
        private Random _rng = new Random();

        public World(SimulationConfig config)
        {
            _config = config;
            Width = config.MapWidth;
            Height = config.MapHeight;
            Center = new Point2D(Width / 2, Height / 2);

            FoodStore = config.InitialAnts;

            InitializeWorld();
        }

        public void AddObserver(ISimulationObserver observer) => _observers.Add(observer);
        public void RemoveObserver(ISimulationObserver observer) => _observers.Remove(observer);
        public void NotifyObservers()
        {
            foreach (var obs in _observers) obs.OnSimulationTick();
        }
        public IWorldIterator GetWorldIterator()
        {
            return new WorldIterator(this);
        }

        private void InitializeWorld()
        {
            for (int i = 0; i < _config.InitialAnts; i++)
            {
                Ants.Add(new AntContext(Center.X, Center.Y));
            }

            GenerateFoodCluster(50, 50, 20);
            GenerateFoodCluster(Width - 50, Height - 50, 20);
            GenerateFoodCluster(Width - 100, 100, 20);

            for (int i = 0; i < 2; i++)
            {
                var threat = new EntityViewModel { X = _rng.Next(0, (int)Width), Y = _rng.Next(0, (int)Height), ColorBrush = Brushes.Red, Size = 8 };
                Threats.Add(threat);

                double angle = _rng.NextDouble() * 2 * Math.PI;
                _threatVelocities[threat] = new Vector2D(Math.Cos(angle), Math.Sin(angle));
            }
        }

        private void GenerateFoodCluster(double x, double y, int count)
        {
            for (int i = 0; i < count; i++)
            {
                Food.Add(new EntityViewModel
                {
                    X = x + _rng.Next(-20, 20),
                    Y = y + _rng.Next(-20, 20),
                    ColorBrush = Brushes.Green,
                    Size = 6
                });
            }
        }

        public void DropFood(double x, double y)
        {
            Food.Add(new EntityViewModel
            {
                X = x,
                Y = y,
                ColorBrush = Brushes.Green,
                Size = 6
             });
         }

        public void Tick()
        {
            for (int i = Ants.Count - 1; i >= 0; i--)
            {
                Ants[i].Update(this);
                if (Ants[i].IsDead) Ants.RemoveAt(i);
            }

            foreach (var threat in Threats)
            {
                if (!_threatVelocities.ContainsKey(threat))
                    _threatVelocities[threat] = new Vector2D(1, 0);

                Vector2D velocity = _threatVelocities[threat];
                double currentAngle = Math.Atan2(velocity.Y, velocity.X);
                double angleChange = (_rng.NextDouble() * 0.4) - 0.2;
                double newAngle = currentAngle + angleChange;

                Vector2D newVelocity = new Vector2D(Math.Cos(newAngle), Math.Sin(newAngle));

                double speed = 1.5;
                double nextX = threat.X + newVelocity.X * speed;
                double nextY = threat.Y + newVelocity.Y * speed;
                double distToHome = (new Point2D(Center.X, Center.Y) - new Point2D(nextX, nextY)).Length;

                bool hitWallX = false;
                bool hitWallY = false;

                if (nextX <= 0 || nextX >= Width)
                {
                    newVelocity.X *= -1;
                    nextX = nextX <= 0 ? 0 : Width;
                    hitWallX = true;
                }
                if (nextY <= 0 || nextY >= Height)
                {
                    newVelocity.Y *= -1;
                    nextY = nextY <= 0 ? 0 : Height;
                    hitWallY = true;
                }

                if (distToHome < 100)
                {
                    newVelocity = -newVelocity;
                }

                _threatVelocities[threat] = newVelocity;
                threat.X = nextX;
                threat.Y = nextY;
            }

            for (int i = Pheromones.Count - 1; i >= 0; i--)
            {
                Pheromones[i].Intensity -= 0.005;
                if (Pheromones[i].Intensity <= 0) Pheromones.RemoveAt(i);
            }

            if (FoodStore > Ants.Count * 1.5 && FoodStore >= _config.SpawnCost)
            {
                FoodStore -= _config.SpawnCost;
                Ants.Add(new AntContext(Center.X, Center.Y));
            }

            if (_rng.Next(0, 400) < 1)
            {
                GenerateFoodCluster(_rng.Next(20, (int)Width - 20), _rng.Next(20, (int)Height - 20), _rng.Next(3, 21));
            }

            NotifyObservers();
        }

        public EntityViewModel TryEatFood(double x, double y)
        {
            var food = Food.FirstOrDefault(f => Math.Sqrt(Math.Pow(f.X - x, 2) + Math.Pow(f.Y - y, 2)) < 10);
            if (food != null) { Food.Remove(food); return food; }
            return null;
        }

        public bool IsHome(double x, double y) => Math.Sqrt(Math.Pow(Center.X - x, 2) + Math.Pow(Center.Y - y, 2)) < 25;
        public void DepositFood(int amount) => FoodStore += amount;
        public bool ConsumeRation() { if (FoodStore > 0) { FoodStore--; return true; } return false; }


        public void AddPheromone(double x, double y)
        {
            if (_rng.NextDouble() > 0.1) return;
            Pheromones.Add(new PheromoneViewModel { X = x, Y = y, Intensity = 1.0 });
        }

        public bool IsThreatNear(double x, double y)
        {
            return Threats.Any(t => Math.Sqrt(Math.Pow(t.X - x, 2) + Math.Pow(t.Y - y, 2)) < 50);
        }

        public EntityViewModel GetNearestThreat(double x, double y)
        {
            return Threats.OrderBy(t => Math.Sqrt(Math.Pow(t.X - x, 2) + Math.Pow(t.Y - y, 2))).FirstOrDefault();
        }
        public ColonyMemento CreateMemento()
        {
            return new ColonyMemento
            {
                FoodStore = this.FoodStore,
                AntSnapshots = this.Ants.Select(a => a.GetSnapshot()).ToList(),
                FoodSnapshots = this.Food.Select(f => new Point2D(f.X, f.Y)).ToList()
            };
        }

        public void RestoreMemento(ColonyMemento memento)
        {
            this.FoodStore = memento.FoodStore;

            this.Food.Clear();
            foreach (var p in memento.FoodSnapshots)
                this.Food.Add(new EntityViewModel { X = p.X, Y = p.Y, ColorBrush = Brushes.Green, Size = 6 });

            this.Ants.Clear();
            foreach (var snap in memento.AntSnapshots)
            {
                var ant = new AntContext(snap.X, snap.Y);
                ant.Restore(snap);
                this.Ants.Add(ant);
            }
            this.Pheromones.Clear();
        }
    }
}
