using Lab3.Individual;
using Lab3.Mementos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab3.States
{
    public class SusceptibleState : IHealthState
    {
        private Dictionary<int, double> _exposureTimes = new Dictionary<int, double>();
        private const double InfectionDistance = 2.0;
        private const double RequiredContactTime = 3.0;

        public SusceptibleState(Dictionary<int, double> exposureData = null)
        {
            if (exposureData != null) _exposureTimes = new Dictionary<int, double>(exposureData);
        }

        public void Update(Person context, double deltaTime) {  }

        public void HandleContact(Person context, Person other, double distance)
        {
            var otherStatus = other.CurrentState.GetStatus();

            if (otherStatus == HealthStatus.NoSymptoms || otherStatus == HealthStatus.Symptoms)
            {
                if (distance <= InfectionDistance)
                {
                    if (!_exposureTimes.ContainsKey(other.Id))
                    {
                        _exposureTimes[other.Id] = 0;
                    }

                    _exposureTimes[other.Id] += 0.04;

                    if (_exposureTimes[other.Id] >= RequiredContactTime)
                    {
                        AttemptInfection(context, otherStatus);
                        _exposureTimes.Remove(other.Id);
                    }
                }
                else
                {
                    if (_exposureTimes.ContainsKey(other.Id))
                    {
                        _exposureTimes.Remove(other.Id);
                    }
                }
            }
        }

        private void AttemptInfection(Person context, HealthStatus sourceStatus)
        {
            Random rng = new Random();
            double chance = 0;

            if (sourceStatus == HealthStatus.NoSymptoms) chance = 0.5;
            else if (sourceStatus == HealthStatus.Symptoms) chance = 1.0;

            if (rng.NextDouble() < chance)
            {
                context.SetState(new InfectedState());
            }
        }

        public HealthStatus GetStatus() => HealthStatus.Susceptible;

        public StateMemento CreateMemento()
        {
            return new StateMemento
            {
                Status = HealthStatus.Susceptible,
                ExposureTimes = new Dictionary<int, double>(_exposureTimes)
            };
        }
    }
}
