using Lab3.Individual;
using Lab3.Mementos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab3.States
{
    public class InfectedState : IHealthState
    {
        private double _timeToRecover;
        private bool _hasSymptoms;

        public InfectedState(double? remainingTime = null, bool? symptoms = null)
        {
            Random rng = new Random();

            if (symptoms.HasValue) _hasSymptoms = symptoms.Value;
            else _hasSymptoms = rng.NextDouble() > 0.5;

            if (remainingTime.HasValue) _timeToRecover = remainingTime.Value;
            else _timeToRecover = 20.0 + (rng.NextDouble() * 10.0);
        }

        public void Update(Person context, double deltaTime)
        {
            _timeToRecover -= deltaTime;
            if (_timeToRecover <= 0)
            {
                context.SetState(new ImmuneState());
            }
        }

        public void HandleContact(Person context, Person other, double distance){  }

        public HealthStatus GetStatus()
        {
            return _hasSymptoms ? HealthStatus.Symptoms : HealthStatus.NoSymptoms;
        }

        public StateMemento CreateMemento()
        {
            return new StateMemento
            {
                Status = _hasSymptoms ? HealthStatus.Symptoms : HealthStatus.NoSymptoms,
                TimeRemaining = _timeToRecover
            };
        }
    }
}
