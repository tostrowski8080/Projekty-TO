using System;
using System.Linq;
using System.Threading.Tasks;
using TO_Lab1.Exchange;
using TO_Lab1.Interfaces;
using TO_Lab1.Views;
using TO_Lab1.Repositories;
using TO_Lab1.Documents;

namespace TO_Lab1.Controllers
{
    public class ExchangeController
    {
        public readonly UserInterface _ui;
        public readonly RemoteRepository _repo;
        public readonly Encodings.Encoding _encoding;
        public readonly Document _document;
        public readonly Exchanger _exchanger;

        public ExchangeTable? _cachedTable;

        public ExchangeController(UserInterface ui, RemoteRepository repo, Encodings.Encoding encoding, Document document, Exchanger exchanger)
        {
            _ui = ui;
            _repo = repo;
            _encoding = encoding;
            _document = document;
            _exchanger = exchanger;
        }

        public async Task StartAsync()
        {
            while (true)
            {
                var action = await _ui.getActionAsync(this);
                if (action == null)
                    continue;

                await action.executeAsync(this);
            }
        }

        public async Task<ExchangeTable> GetExchangeTableAsync()
        {
            if (_cachedTable != null)
                return _cachedTable;

            _ui.Display(new InfoView("Updating exchange rates..."));
            try
            {
                byte[] data = await _repo.GetAsync("https://static.nbp.pl/dane/kursy/xml/LastA.xml");
                string xml = _encoding.getString(data);

                _cachedTable = _document.getTable(xml);
                _ui.Display(new InfoView($"Exchange rates updated: {_cachedTable.timestamp:G}\n"));
            }
            catch (Exception ex)
            {
                _ui.Display(new ErrorView($"Couldn't update exchange rates: {ex.Message}"));
            }

            return _cachedTable!;
        }
    }
}
