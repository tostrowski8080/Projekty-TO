using Lab4.Observer;
using Lab4.State;
using Lab4.Vector;
using System.Collections.Generic;

namespace Lab4.Objects
{
    public class Car : Observer.IObservable<Car>
    {
        public int Id { get; private set; }
        public JRG HomeBase { get; private set; }
        public ICarState CurrentState { get; private set; }
        public Vector2D Position { get; set; }

        private List<Observer.IObserver<Car>> _observers = new List<Observer.IObserver<Car>>();

        public Car(int id, JRG homeBase)
        {
            Id = id;
            HomeBase = homeBase;
            Position = homeBase.Position;
            CurrentState = new FreeState();
        }

        public void SetState(ICarState newState)
        {
            CurrentState = newState;
            Notify(this);
        }

        public void Update(double deltaTime)
        {
            CurrentState.Update(this, deltaTime);
        }

        public void Attach(Observer.IObserver<Car> observer)
        {
            if (!_observers.Contains(observer))
                _observers.Add(observer);
        }

        public void Detach(Observer.IObserver<Car> observer)
        {
            _observers.Remove(observer);
        }

        public void Notify(Car data)
        {
            foreach (var observer in new List<Observer.IObserver<Car>>(_observers))
            {
                observer.OnUpdate(data);
            }
        }
    }
}