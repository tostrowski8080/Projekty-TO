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
    public class FleeingState : AntState
    {
        private IMovementStrategy _movement = new FleeStrategy();
        private int _fleeTimer = 0;

        public override void Update(World world)
        {
            Vector2D move = _movement.CalculateMove(_ant, world);
            _ant.Move(move * 2.0, world.Width, world.Height);
            _fleeTimer++;

            if (_fleeTimer > 20 && !world.IsThreatNear(_ant.X, _ant.Y))
            {
                _ant.TransitionTo(new ForagingState());
            }
        }
    }
}
