using System;
using System.Globalization;
using System.Threading.Tasks;
using TO_Lab1.Views;
using TO_Lab1.Actions;
using TO_Lab1.Controllers;

namespace TO_Lab1.Interfaces
{
    public class ConsoleUI : UserInterface
    {
        public void Display(View v)
        {
            Console.WriteLine(v.render());
        }

        public async Task<Actions.Action?> getActionAsync(ExchangeController controller)
        {
            Console.Write("Input command (help/currencies/exchange/update/exit): ");
            string? command = Console.ReadLine()?.Trim().ToLower();

            switch (command)
            {
                case "help":
                    return new HelpAction();

                case "currencies":
                    return new CurrenciesAction();

                case "update":
                    return new UpdateAction();

                case "exit":
                    return new ExitAction();

                case "exchange":
                    var table = await controller.GetExchangeTableAsync();
                    var currencies = table.GetAllCurrencies();

                    string from;
                    do
                    {
                        Console.Write("Input source currency: ");
                        from = (Console.ReadLine() ?? "").Trim().ToUpper();
                    } while (!currencies.Contains(from));

                    string to;
                    do
                    {
                        Console.Write("Input target currency: ");
                        to = (Console.ReadLine() ?? "").Trim().ToUpper();
                    } while (!currencies.Contains(to));

                    double amount;
                    while (true)
                    {
                        Console.Write("Input amount to exchange: ");
                        if (double.TryParse(Console.ReadLine(), NumberStyles.Any, CultureInfo.InvariantCulture, out amount) && amount > 0)
                            break;
                    }

                    return new ExchangeAction(from, to, amount);

                default:
                    Console.WriteLine("Unknown command. Type 'help' to get list of available commands");
                    return null;
            }
        }
    }
}
