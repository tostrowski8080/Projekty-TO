using Lab4.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab4.State
{
    public class FreeState : ICarState
    {
        public void Update(Car car, double deltaTime)
        {
            car.Position = car.HomeBase.Position;
        }
    }
}
