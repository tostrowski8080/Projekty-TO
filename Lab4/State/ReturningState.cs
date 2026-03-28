using Lab4.Objects;
using Lab4.Vector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab4.State
{
    public class ReturningState : ICarState
    {
        private readonly Vector2D _target;
        private double _travelTimeRemaining;
        private readonly double _totalTime;
        private readonly Vector2D _startPos;

        public ReturningState(Vector2D start, Vector2D target, double duration)
        {
            _startPos = start;
            _target = target;
            _totalTime = duration;
            _travelTimeRemaining = duration;
        }

        public void Update(Car car, double deltaTime)
        {
            _travelTimeRemaining -= deltaTime;

            double progress = 1.0 - (_travelTimeRemaining / _totalTime);
            if (progress > 1.0) progress = 1.0;

            car.Position = _startPos + (_target - _startPos) * progress;

            if (_travelTimeRemaining <= 0)
            {
                car.SetState(new FreeState());
            }
        }
    }
}
