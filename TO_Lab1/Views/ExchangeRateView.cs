using System;
using TO_Lab1.Exchange;

namespace TO_Lab1.Views
{
    public class ExchangeRateView : View
    {
        private readonly ExchangeRate _rate;
        private readonly double _value;
        private readonly double _inputAmount;

        public ExchangeRateView(ExchangeRate rate, double value, double inputAmount)
        {
            _rate = rate;
            _value = value;
            _inputAmount = inputAmount;
        }

        public string render()
        {
            return ($"Exchange: {_inputAmount:F2} {_rate.Source} = {_value:F2} {_rate.Target}");
        }
    }
}
