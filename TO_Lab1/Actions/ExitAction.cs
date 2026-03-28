using System;
using System.Threading.Tasks;
using TO_Lab1.Controllers;

namespace TO_Lab1.Actions
{
    public class ExitAction : Action
    {
        public async Task executeAsync(ExchangeController controller)
        {
            Environment.Exit(0);
            await Task.CompletedTask;
        }
    }
}
