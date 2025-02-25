using CurrencyConverter.Converter;
using CurrencyConverter.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace CurrencyConverter.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly Wallet _wallet = new Wallet();
        private readonly ConverterCurrency _converter = new ConverterCurrency();

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
            _wallet.Balances["BTC"] = 1;
            _wallet.Balances["XRP"] = 15000;
            _wallet.Balances["XMR"] = 50;
            _wallet.Balances["DSH"] = 30;
            _wallet.Balances["USD"] = 0;
        }

        public async Task<IActionResult> Index()
        {
            List<string> currencies = _wallet.Currencies.ToList();
            Dictionary<string, double> balances = new Dictionary<string, double>(_wallet.Balances);
            for (int i = 0; i < currencies.Count(); i++)
            {
                string ccy1 = currencies[i];
                for (int j = i + 1; j < currencies.Count(); j++)
                {
                    string ccy2 = currencies[j];
                    double c1 = await _converter.Convert(ccy2, ccy1);
                    double c2 = 1 / c1;
                    balances[ccy1] += c1 * _wallet.Balances[ccy2];
                    balances[ccy2] += c2 * _wallet.Balances[ccy1];
                }
            }
            ViewBag.Balances = balances;
            return View();
        }
    }
}