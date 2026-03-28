using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TO_Lab1.Exchange
{
    public class Exchanger
    {
        public double exchange(string from, string to, double amount, ExchangeTable _table)
        {
            double fromRate = _table.GetRate(from);
            double toRate = _table.GetRate(to);
            return amount * fromRate / toRate;
        }
    }
}
