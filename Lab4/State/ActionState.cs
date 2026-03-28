using Lab4.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab4.State
{
    public class ActionState : ICarState
    {
        private double _actionTimeRemaining;

        public ActionState(double duration)
        {
            _actionTimeRemaining = duration;
        }

        public void Update(Car car, double deltaTime)
        {
            _actionTimeRemaining -= deltaTime;
            if (_actionTimeRemaining <= 0)
            {
                Random rng = new Random();
                double returnTime = rng.NextDouble() * 3.0;
                car.SetState(new ReturningState(car.Position, car.HomeBase.Position, returnTime));
            }
        }
    }
}
