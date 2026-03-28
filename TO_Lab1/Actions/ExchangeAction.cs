using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TO_Lab1.Controllers;
using TO_Lab1.Exchange;
using TO_Lab1.Views;

namespace TO_Lab1.Actions
{
    public class ExchangeAction : Action
    {
        private readonly string _source;
        private readonly string _target;
        private readonly double _amount;

        public ExchangeAction(string source, string target, double amount)
        {
            _source = source.ToUpper();
            _target = target.ToUpper();
            _amount = amount;
        }

        public async Task executeAsync(ExchangeController controller)
        {
            try
            {
                var table = await controller.GetExchangeTableAsync();

                double result = controller._exchanger.exchange(_source, _target, _amount, table);
                var rate = new ExchangeRate(_source, _target, table.GetRate(_source) / table.GetRate(_target));

                controller._ui.Display(new ExchangeRateView(rate, result, _amount));
                controller._ui.Display(new InfoView($"(Exchange rates from: {table.timestamp:G})"));
            }
            catch (Exception ex)
            {
                controller._ui.Display(new ErrorView("Error during exchange: " + ex.Message));
            }
            await Task.CompletedTask;
        }
    }
}
