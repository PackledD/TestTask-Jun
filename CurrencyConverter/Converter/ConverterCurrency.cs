using CurrencyConverter.Models;
using System.Text.Json;

namespace CurrencyConverter.Converter
{
    public class ConverterCurrency : IDisposable
    {
        private HttpClient _httpClient;

        public ConverterCurrency()
        {
            _httpClient = new HttpClient();
        }

        public Task<double> Convert(string ccy1, string ccy2)
        {
            return Convert(new ConvertRateRequest { Ccy1 = ccy1, Ccy2 = ccy2 });
        }

        public async Task<double> Convert(ConvertRateRequest rq)
        {
            var data = await _httpClient.PostAsJsonAsync("https://api-pub.bitfinex.com/v2/calc/fx", rq);
            if (!data.IsSuccessStatusCode)
            {
                throw new HttpRequestException("Can't request data");
            }
            using (var doc = JsonDocument.Parse(await data.Content.ReadAsStringAsync()))
            {
                double res;
                if (doc.RootElement.EnumerateArray().ToArray()[0].TryGetDouble(out res))
                {
                    return res;
                }
                throw new ArgumentException("Currency doesn't exist");
            }
        }

        public void Dispose()
        {
            _httpClient?.Dispose();
        }
    }
}
