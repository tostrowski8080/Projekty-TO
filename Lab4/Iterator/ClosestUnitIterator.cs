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
    public class ClosestUnitIterator : IEnumerator<JRG>
    {
        private readonly List<JRG> _sortedUnits;
        private int _position = -1;

        public ClosestUnitIterator(List<JRG> allUnits, Vector2D targetLocation)
        {
            _sortedUnits = allUnits.OrderBy(u => Vector2D.Distance(u.Position, targetLocation)).ToList();
        }

        public JRG Current => _sortedUnits[_position];

        object IEnumerator.Current => Current;

        public void Dispose() { }

        public bool MoveNext()
        {
            _position++;
            return _position < _sortedUnits.Count;
        }

        public void Reset()
        {
            _position = -1;
        }
    }
}
