using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TO_Lab1.Exchange
{
    public class ExchangeTable
    {
        private readonly Dictionary<string, double> _rates;
        public DateTime timestamp { get; }

        public ExchangeTable(Dictionary<string, double> rates)
        {
            _rates = rates;
            timestamp = DateTime.Now;
        }

        public double GetRate(string currency)
        {
            if (currency.ToUpper() == "PLN")
                return 1.0;

            return _rates.TryGetValue(currency.ToUpper(), out var value) ? value : 0.0;
        }

        public IEnumerable<string> GetAllCurrencies()
        {
            return _rates.Keys;
        }
    }
}
