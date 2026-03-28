using Lab4.State;
using Lab4.Vector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab4.Objects
{
    public class JRG
    {
        public string Name { get; private set; }
        public Vector2D Position { get; private set; }
        public List<Car> Cars { get; private set; }

        public JRG(string name, double lat, double lon)
        {
            Name = name;
            Position = new Vector2D(lon, lat);
            Cars = new List<Car>();
            for (int i = 1; i <= 5; i++)
            {
                Cars.Add(new Car(i, this));
            }
        }

        public List<Car> GetFreeCars()
        {
            return Cars.Where(t => t.CurrentState is FreeState).ToList();
        }
    }
}
