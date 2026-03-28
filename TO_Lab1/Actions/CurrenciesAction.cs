using System.Threading.Tasks;
using TO_Lab1.Controllers;
using TO_Lab1.Views;

namespace TO_Lab1.Actions
{
    public class CurrenciesAction : Action
    {
        public async Task executeAsync(ExchangeController controller)
        {
            var table = await controller.GetExchangeTableAsync();
            var list = string.Join(", ", table.GetAllCurrencies().OrderBy(c => c));
            controller._ui.Display(new InfoView("Available currencies:\n" + list + "\n"));
            await Task.CompletedTask;
        }
    }
}
