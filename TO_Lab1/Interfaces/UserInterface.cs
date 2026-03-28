using System.Threading.Tasks;
using TO_Lab1.Views;
using TO_Lab1.Actions;
using TO_Lab1.Controllers;

namespace TO_Lab1.Interfaces
{
    public interface UserInterface
    {
        void Display(View v);
        Task<Actions.Action?> getActionAsync(ExchangeController controller);
    }
}
