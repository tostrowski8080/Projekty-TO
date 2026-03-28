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
    public class ReturningState : AntState
    {
        private IMovementStrategy _movement = new ReturnHomeStrategy();

        public override void Update(World world)
        {
            if (world.IsThreatNear(_ant.X, _ant.Y))
            {
                _ant.RemoveCarrierDecorator();
                world.DropFood(_ant.X, _ant.Y);
                _ant.TransitionTo(new FleeingState());
                return;
            }

            Vector2D move = _movement.CalculateMove(_ant, world);
            _ant.Move(move, world.Width, world.Height);

            world.AddPheromone(_ant.X, _ant.Y);

            if (world.IsHome(_ant.X, _ant.Y))
            {
                world.DepositFood(2);
                bool survived = world.ConsumeRation();

                if (!survived)
                {
                    _ant.IsDead = true;
                }
                else
                {
                    _ant.RemoveCarrierDecorator();
                    _ant.Direction = -_ant.Direction;
                    _ant.TransitionTo(new ForagingState());
                }
            }
        }
    }
 }
