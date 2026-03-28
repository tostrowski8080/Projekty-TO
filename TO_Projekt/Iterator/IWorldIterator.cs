using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TO_Projekt.Models;

namespace TO_Projekt.Iterator
{
    public interface IWorldIterator
    {
        bool HasNext();
        EntityViewModel Next();
        void Reset();
    }
}
