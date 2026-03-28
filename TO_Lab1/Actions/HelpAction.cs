using System.Threading.Tasks;
using TO_Lab1.Controllers;
using TO_Lab1.Views;

namespace TO_Lab1.Actions
{
    public class HelpAction : Action
    {
        public async Task executeAsync(ExchangeController controller)
        {
            controller._ui.Display(new InfoView(
                "\nAvailable commands:\n" +
                "help          - shows list of commands\n" +
                "currencies    - shows list of available currencies\n" +
                "exchange      - starts currency exchange\n" +
                "update        - updates exchange rate table\n" +
                "exit          - exits the program\n"
            ));
            await Task.CompletedTask;
        }
    }
}
