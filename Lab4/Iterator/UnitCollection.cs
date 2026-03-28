using Lab4.Objects;
using Lab4.Vector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab4.Iterator
{
    public class UnitCollection : IEnumerable<JRG>
    {
        private readonly List<JRG> _units;
        private readonly Vector2D _target;

        public UnitCollection(List<JRG> units, Vector2D target)
        {
            _units = units;
            _target = target;
        }

        public IEnumerator<JRG> GetEnumerator()
        {
            return new ClosestUnitIterator(_units, _target);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
