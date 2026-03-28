using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TO_Lab1.Exchange;

namespace TO_Lab1.Documents
{
    public interface Document
    {
        ExchangeTable getTable(string contents);
    }
}
