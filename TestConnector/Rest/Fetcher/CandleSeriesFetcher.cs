using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using ConnectorTest.Rest.Interface;
using ConnectorTest.Utils;
using TestConnector.Utils;
using TestHQ;

namespace ConnectorTest.Rest.Fetcher
{
    internal class CandleSeriesFetcher : BaseJsonFetcher<Candle>, ICandleSeriesFetcher
    {
        public async Task<IEnumerable<Candle>> GetCandleSeriesAsync(string pair, int periodInSec, DateTimeOffset? from, DateTimeOffset? to, long? count)
        {
            var period = PeriodBuilder.FromSec(periodInSec);
            string url = $"https://api-pub.bitfinex.com/v2/candles/trade:{period}:{pair}/hist";
            List<string> urlParams = new List<string>();
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
            var res = await FetchJsonCollectionAsync(UrlBuilder.Build(url, urlParams));
            if (res == null)
            {
                throw new HttpRequestException("Can't fetch data");
            }
            foreach (var item in res)
            {
                item.Pair = pair;
            }
            return res;
        }

        protected override IEnumerable<Candle> ParseEntityCollection(string json)
        {
            using (var doc = JsonDocument.Parse(json))
            {
                return Parser.ParseCandleEnumerable(doc.RootElement);
            }
        }
    }
}
