using Lab3.Mementos;
using Lab3.States;
using Lab3.Vector;
using Lab3.Individual;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Lab3.Simulation
{
    public class SimulationEngine
    {
        public List<Person> Population { get; private set; }
        public double Width { get; private set; }
        public double Height { get; private set; }
        public double CurrentTime { get; private set; }

        private int _nextId = 0;
        private Random _rng = new Random();
        private int _targetPopulation;
        private double _initialImmunityChance;

        public SimulationEngine(double width, double height)
        {
            Width = width;
            Height = height;
            Population = new List<Person>();
        }

        public void Init(int count, double immunityChance, double infectedChance)
        {
            Population.Clear();
            _nextId = 0;
            CurrentTime = 0;
            _targetPopulation = count;
            _initialImmunityChance = immunityChance;

            for (int i = 0; i < count; i++)
            {
                bool startInfected = _rng.NextDouble() < infectedChance;
                SpawnIndividual(atBoundary: false, forceInfected: startInfected);
            }
        }

        private void SpawnIndividual(bool atBoundary, bool forceInfected = false)
        {
            Vector2D pos = new Vector2D(0, 0);
            Vector2D vel;

            double speed = _rng.NextDouble() * 2.5;
            double angle = _rng.NextDouble() * 2 * Math.PI;
            vel = new Vector2D(Math.Cos(angle), Math.Sin(angle)) * speed;

            if (atBoundary)
            {
                int edge = _rng.Next(4);
                switch (edge)
                {
                    case 0: // Top
                        pos = new Vector2D(_rng.NextDouble() * Width, 0.1);
                        vel.Y = Math.Abs(vel.Y);
                        break;
                    case 1: // Right
                        pos = new Vector2D(Width - 0.1, _rng.NextDouble() * Height);
                        vel.X = -Math.Abs(vel.X);
                        break;
                    case 2: // Bottom
                        pos = new Vector2D(_rng.NextDouble() * Width, Height - 0.1);
                        vel.Y = -Math.Abs(vel.Y);
                        break;
                    case 3: // Left
                        pos = new Vector2D(0.1, _rng.NextDouble() * Height);
                        vel.X = Math.Abs(vel.X);
                        break;
                }
            }
            else
            {
                pos = new Vector2D(_rng.NextDouble() * Width, _rng.NextDouble() * Height);
            }

            IHealthState state;
            if (forceInfected) state = new InfectedState();
            else state = (_rng.NextDouble() < _initialImmunityChance) ? (IHealthState)new ImmuneState() : new SusceptibleState();

            Population.Add(new Person(++_nextId, pos, vel, state));
        }

        public void Update(double deltaTime)
        {
            CurrentTime += deltaTime;

            foreach (var person in Population)
            {
                person.Update(deltaTime, Width, Height);
            }

            Population.RemoveAll(p => p.MarkedForRemoval);

            while (Population.Count < _targetPopulation)
            {
                bool infected = _rng.NextDouble() < 0.10;
                SpawnIndividual(atBoundary: true, forceInfected: infected);
            }

            for (int i = 0; i < Population.Count; i++)
            {
                for (int j = i + 1; j < Population.Count; j++)
                {
                    var p1 = Population[i];
                    var p2 = Population[j];

                    double dist = Vector2D.Distance(p1.Position, p2.Position);

                    if (dist <= 2.1)
                    {
                        p1.CurrentState.HandleContact(p1, p2, dist);
                        p2.CurrentState.HandleContact(p2, p1, dist);
                    }
                }
            }
        }

        public (int Susceptible, int Infected, int Immune) GetStats()
        {
            int s = 0, i = 0, im = 0;
            foreach (var p in Population)
            {
                var status = p.CurrentState.GetStatus();
                if (status == HealthStatus.Immune) im++;
                else if (status == HealthStatus.Susceptible) s++;
                else i++;
            }
            return (s, i, im);
        }

        public string SaveToJson()
        {
            var snapshot = new SimulationSnapshot
            {
                CurrentTime = this.CurrentTime,
                Width = this.Width,
                Height = this.Height,
                Individuals = Population.Select(p => p.SaveState()).ToList()
            };
            return JsonSerializer.Serialize(snapshot, new JsonSerializerOptions { WriteIndented = true });
        }

        public void LoadFromJson(string json)
        {
            var snapshot = JsonSerializer.Deserialize<SimulationSnapshot>(json);
            if (snapshot == null) return;

            this.CurrentTime = snapshot.CurrentTime;
            this.Width = snapshot.Width;
            this.Height = snapshot.Height;
            this.Population.Clear();

            int maxId = 0;
            foreach (var mem in snapshot.Individuals)
            {
                var p = new Person(mem.Id, new Vector2D(0, 0), new Vector2D(0, 0), new ImmuneState());
                p.RestoreState(mem);
                this.Population.Add(p);
                if (mem.Id > maxId) maxId = mem.Id;
            }
            _nextId = maxId;
            _targetPopulation = Population.Count;
        }
    }
}
