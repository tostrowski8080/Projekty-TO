using System.Threading.Tasks;
using TO_Lab1.Controllers;
using TO_Lab1.Views;

namespace TO_Lab1.Actions
{
    public class UpdateAction : Action
    {
        public async Task executeAsync(ExchangeController controller)
        {
            controller._ui.Display(new InfoView("Updating exchange rates..."));
            try
            {
                byte[] data = await controller._repo.GetAsync("https://static.nbp.pl/dane/kursy/xml/LastA.xml");
                string xml = controller._encoding.getString(data);

                controller._cachedTable = controller._document.getTable(xml);
                controller._ui.Display(new InfoView($"Exchange rates updated: {controller._cachedTable.timestamp:G}\n"));
            }
            catch (Exception ex)
            {
                controller._ui.Display(new ErrorView($"Couldn't update exchange rates: {ex.Message}"));
            }
            await Task.CompletedTask;
        }
    }
}
