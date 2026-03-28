using Lab3.Individual;
using Lab3.Mementos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab3.States
{
    public class ImmuneState : IHealthState
    {
        public void Update(Person context, double deltaTime) { }

        public void HandleContact(Person context, Person other, double distance) { }

        public HealthStatus GetStatus() => HealthStatus.Immune;

        public StateMemento CreateMemento()
        {
            return new StateMemento { Status = HealthStatus.Immune };
        }
    }
}
