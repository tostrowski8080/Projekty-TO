using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TO_Lab1.Exchange
{
    public class ExchangeRate
    {
        public string Source { get; }
        public string Target { get; }
        public double Rate { get; }

        public ExchangeRate(string source, string target, double rate)
        {
            Source = source;
            Target = target;
            Rate = rate;
        }
    }
}
