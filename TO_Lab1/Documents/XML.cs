using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using TO_Lab1.Exchange;

namespace TO_Lab1.Documents
{
    public class XML : Document
    {
        public ExchangeTable getTable(string contents)
        {
            var rates = new Dictionary<string, double>();
            var doc = new XmlDocument();
            doc.LoadXml(contents);

            var nodes = doc.GetElementsByTagName("pozycja");
            foreach (XmlNode node in nodes)
            {
                string code = node["kod_waluty"]!.InnerText.Trim();
                string rateStr = node["kurs_sredni"]!.InnerText.Trim().Replace(",", ".");

                if (double.TryParse(rateStr, NumberStyles.Any, CultureInfo.InvariantCulture, out double rate))
                {
                    rates[code.ToUpper()] = rate;
                }
            }
            rates["PLN"] = 1.0;

            return new ExchangeTable(rates);
        }
    }
}
