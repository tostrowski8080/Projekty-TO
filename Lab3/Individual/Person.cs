using Lab3.Vector;
using Lab3.States;
using Lab3.Mementos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab3.Individual
{
    public class Person
    {
        public int Id { get; private set; }
        public Vector2D Position { get; set; }
        public Vector2D Velocity { get; set; }
        public IHealthState CurrentState { get; private set; }
        public bool MarkedForRemoval { get; private set; } = false;

        private static readonly Random _rng = new Random();
        private readonly double _maxSpeed;

        public Person(int id, Vector2D startPos, Vector2D startVel, IHealthState initialState, double maxSpeed = 2.5)
        {
            Id = id;
            Position = startPos;
            Velocity = startVel;
            CurrentState = initialState;
            _maxSpeed = maxSpeed;
        }

        public void SetState(IHealthState newState)
        {
            CurrentState = newState;
        }

        public void Update(double deltaTime, double areaWidth, double areaHeight)
        {
            ApplyRandomPerturbation();
            Move(deltaTime);
            CurrentState.Update(this, deltaTime);
            EnforceBoundaries(areaWidth, areaHeight);
        }

        private void Move(double deltaTime)
        {
            Position += Velocity * deltaTime;
        }

        private void ApplyRandomPerturbation()
        {
            if (_rng.NextDouble() < 0.05)
            {
                double angle = (_rng.NextDouble() * 2 * Math.PI);
                Vector2D perturbation = new Vector2D(Math.Cos(angle), Math.Sin(angle)) * 0.5;
                Velocity += perturbation;

                if (Velocity.abs() > _maxSpeed)
                {
                    Velocity = Velocity.Normalize() * _maxSpeed;
                }
            }
        }

        private void EnforceBoundaries(double width, double height)
        {
            bool hitBoundary = false;
            double newVelX = Velocity.X;
            double newVelY = Velocity.Y;
            double newPosX = Position.X;
            double newPosY = Position.Y;

            double pushMargin = 0.2; 
            double minBounceSpeed = 0.5;

            if (Position.X <= 0)
            {
                hitBoundary = true;
                newPosX = pushMargin;
                newVelX = Math.Abs(Velocity.X);
                if (newVelX < minBounceSpeed) newVelX = minBounceSpeed;
            }
            else if (Position.X >= width)
            {
                hitBoundary = true;
                newPosX = width - pushMargin;
                newVelX = -Math.Abs(Velocity.X);
                if (Math.Abs(newVelX) < minBounceSpeed) newVelX = -minBounceSpeed;
            }

            if (Position.Y <= 0)
            {
                hitBoundary = true;
                newPosY = pushMargin;
                newVelY = Math.Abs(Velocity.Y);
                if (newVelY < minBounceSpeed) newVelY = minBounceSpeed;
            }
            else if (Position.Y >= height)
            {
                hitBoundary = true;
                newPosY = height - pushMargin;
                newVelY = -Math.Abs(Velocity.Y);
                if (Math.Abs(newVelY) < minBounceSpeed) newVelY = -minBounceSpeed;
            }

            if (hitBoundary)
            {
                if (ShouldLeave())
                {
                    MarkedForRemoval = true;
                }
                else
                {
                    Position.X = newPosX;
                    Position.Y = newPosY;
                    Velocity.X = newVelX;
                    Velocity.Y = newVelY;
                }
            }
        }

        private bool ShouldLeave() => _rng.NextDouble() < 0.5;

        public PersonMemento SaveState()
        {
            return new PersonMemento
            {
                Id = this.Id,
                PosX = this.Position.X,
                PosY = this.Position.Y,
                VelX = this.Velocity.X,
                VelY = this.Velocity.Y,
                StateData = this.CurrentState.CreateMemento()
            };
        }

        public void RestoreState(PersonMemento m)
        {
            this.Id = m.Id;
            this.Position = new Vector2D(m.PosX, m.PosY);
            this.Velocity = new Vector2D(m.VelX, m.VelY);

            switch (m.StateData.Status)
            {
                case HealthStatus.Immune:
                    this.CurrentState = new ImmuneState();
                    break;
                case HealthStatus.Susceptible:
                    this.CurrentState = new SusceptibleState(m.StateData.ExposureTimes);
                    break;
                case HealthStatus.NoSymptoms:
                    this.CurrentState = new InfectedState(m.StateData.TimeRemaining, false);
                    break;
                case HealthStatus.Symptoms:
                    this.CurrentState = new InfectedState(m.StateData.TimeRemaining, true);
                    break;
            }
        }
    }
}
