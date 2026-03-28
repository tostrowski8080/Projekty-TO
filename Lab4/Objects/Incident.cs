using Lab4.Observer;
using Lab4.State;
using Lab4.Strategy;
using Lab4.Vector;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Lab4.Objects
{
    public enum IncidentType { PZ, MZ, AF }

    public class Incident : Observer.IObserver<Car>, Observer.IObservable<Incident>
    {
        public Guid Id { get; private set; } = Guid.NewGuid();
        public Vector2D Position { get; private set; }
        public IIncidentStrategy Strategy { get; private set; }

        public bool IsResolved { get; private set; } = false;

        private List<Car> _assignedCars = new List<Car>();
        private List<Observer.IObserver<Incident>> _observers = new List<Observer.IObserver<Incident>>();
        private Random _rng = new Random();

        private bool _removalRequested = false;

        public Incident(Vector2D pos, IIncidentStrategy strategy)
        {
            Position = pos;
            Strategy = strategy;
        }

        public void AddAssignedCar(Car car)
        {
            _assignedCars.Add(car);
            car.Attach(this);
        }

        public void OnUpdate(Car car)
        {
            if (!IsResolved)
            {
                bool allArrived = _assignedCars.All(c => c.CurrentState is WaitingState);
                if (allArrived)
                {
                    StartActionPhase();
                }
            }

            if (IsResolved)
            {
                if (car.CurrentState is FreeState)
                {
                    car.Detach(this);
                }

                bool allLeftScene = _assignedCars.All(c => c.CurrentState is ReturningState || c.CurrentState is FreeState);

                if (allLeftScene && !_removalRequested)
                {
                    _removalRequested = true;
                    Notify(this);
                }
            }
        }

        private void StartActionPhase()
        {
            bool isFalseAlarm = _rng.NextDouble() < 0.05;

            if (isFalseAlarm)
            {
                foreach (var car in _assignedCars)
                {
                    double returnTime = _rng.NextDouble() * 3.0;
                    car.SetState(new ReturningState(Position, car.HomeBase.Position, returnTime));
                }
            }
            else
            {
                double sharedActionTime = 5.0 + _rng.NextDouble() * 20.0;
                foreach (var car in _assignedCars)
                {
                    car.SetState(new ActionState(sharedActionTime));
                }
            }

            IsResolved = true;
        }

        public void Attach(Observer.IObserver<Incident> observer)
        {
            if (!_observers.Contains(observer))
                _observers.Add(observer);
        }

        public void Detach(Observer.IObserver<Incident> observer)
        {
            _observers.Remove(observer);
        }

        public void Notify(Incident data)
        {
            foreach (var observer in new List<Observer.IObserver<Incident>>(_observers))
            {
                observer.OnUpdate(data);
            }
        }
    }
}