using Lab3.Individual;
using Lab3.Mementos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab3.States
{
    public enum HealthStatus
    {
        Susceptible,
        NoSymptoms,
        Symptoms,
        Immune
    }

    public interface IHealthState
    {
        void Update(Person context, double deltaTime);
        void HandleContact(Person context, Person other, double distance);
        HealthStatus GetStatus();
        StateMemento CreateMemento();
    }
}
