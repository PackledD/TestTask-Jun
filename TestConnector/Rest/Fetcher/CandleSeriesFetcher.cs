using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestConnector.Rest.Interface;
using TestConnector.Utils;
using TestHQ;

namespace TestConnector.Rest.Fetcher
{
    internal class CandleSeriesFetcher : BaseJsonFetcher<IEnumerable<Candle>>, ICandleSeriesFetcher
    {
        public async Task<IEnumerable<Candle>> GetCandleSeriesAsync(string pair, int periodInSec, DateTimeOffset? from, DateTimeOffset? to, long? count)
        {
            string? period = PeriodFromSec(periodInSec);
            if (period == null)
            {
                throw new ArgumentException("Must take certain values", nameof(periodInSec));
            }
            string url = $"https://api-pub.bitfinex.com/v2/candles/trade:{period}:{pair}/hist";
            List<string> urlParams = new ();
            if (from != null)
            {
                urlParams.Add($"start={from}");
            }
            if (to != null)
            {
                urlParams.Add($"end={to}");
            }
            if (count != null)
            {
                urlParams.Add($"limit={count}");
            }
            var res = await FetchJsonAsync(UrlBuilder.Build(url, urlParams));
            if (res == null)
            {
                throw new HttpRequestException("Can't fetch data");
            }
            return res;
        }

        private string? PeriodFromSec(int sec)
        {
            switch(sec)
            {
                case 60:
                    return "1m";
                case 300:
                    return "5m";
                case 900:
                    return "15m";
                case 1800:
                    return "30m";
                case 3600:
                    return "1h";
                case 10800:
                    return "3h";
                case 21600:
                    return "6h";
                case 43200:
                    return "12h";
                case 86400:
                    return "1D";
                case 604800:
                    return "1W";
                case 1209600:
                    return "14D";
                case 2419200:
                    return "1M";
                default:
                    return null;
            }
        }
    }
}
