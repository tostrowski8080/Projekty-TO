using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TO_Lab1.Controllers;

namespace TO_Lab1.Actions
{

    public interface Action
    {
        Task executeAsync(ExchangeController controller);
    }
}
