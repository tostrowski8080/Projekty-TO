using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using TO_Projekt.Simulation;
using TO_Projekt.Strategy;

namespace TO_Projekt.State
{
    public class ForagingState : AntState
    {
        private IMovementStrategy _movement = new WanderStrategy();

        public override void Update(World world)
        {
            if (world.IsThreatNear(_ant.X, _ant.Y))
            {
                _ant.TransitionTo(new FleeingState());
                return;
            }

            Vector2D move = _movement.CalculateMove(_ant, world);
            _ant.Move(move, world.Width, world.Height);

            var food = world.TryEatFood(_ant.X, _ant.Y);
            if (food != null)
            {
                _ant.ApplyCarrierDecorator();
                _ant.Direction = -_ant.Direction;
                _ant.TransitionTo(new ReturningState());
            }
        }
    }
}
