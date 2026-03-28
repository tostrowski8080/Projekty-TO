using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TO_Projekt.Models;
using TO_Projekt.Simulation;

namespace TO_Projekt.Iterator
{
    public class WorldIterator : IWorldIterator
    {
        private List<List<EntityViewModel>> _collections;
        private int _listIndex = 0;
        private int _elementIndex = 0;

        public WorldIterator(World world)
        {
            _collections = new List<List<EntityViewModel>>();
            _collections.Add(world.Ants.Select(a => a.ViewModel).ToList());
            _collections.Add(world.Threats);
            _collections.Add(world.Food);
        }

        public bool HasNext()
        {
            while (_listIndex < _collections.Count)
            {
                if (_elementIndex < _collections[_listIndex].Count)
                {
                    return true;
                }
                _listIndex++;
                _elementIndex = 0;
            }
            return false;
        }

        public EntityViewModel Next()
        {
            if (!HasNext()) return null;
            var item = _collections[_listIndex][_elementIndex];
            _elementIndex++;
            return item;
        }

        public void Reset()
        {
            _listIndex = 0;
            _elementIndex = 0;
        }
    }
}
