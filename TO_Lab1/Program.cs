using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TO_Lab1.Controllers;
using TO_Lab1.Views;
using TO_Lab1.Documents;
using TO_Lab1.Interfaces;
using TO_Lab1.Repositories;
using TO_Lab1.Encodings;

namespace TO_Lab1
{
        internal class Program
        {
            static async Task Main(string[] args)
            {
            var controller = new ExchangeController(
                new ConsoleUI(),
                new Rest(),
                new ASCII(),
                new XML(),
                new Exchange.Exchanger()
             );

            await controller.StartAsync();
        }
        }
}
