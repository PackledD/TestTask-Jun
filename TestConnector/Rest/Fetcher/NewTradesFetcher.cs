using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using ConnectorTest.Rest.Interface;
using ConnectorTest.Utils;
using TestHQ;

namespace ConnectorTest.Rest.Fetcher
{
    internal class NewTradesFetcher : BaseJsonFetcher<Trade>, INewTradesFetcher
    {
        public async Task<IEnumerable<Trade>> GetNewTradesAsync(string pair, int maxCount)
        {
            string url = $"https://api-pub.bitfinex.com/v2/trades/{pair}/hist";
            List<string> urlParams = new() { $"limit={maxCount}" };
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

        protected override IEnumerable<Trade> ParseEntityCollection(string json)
        {
            List<Trade> res = new();
            using (var doc = JsonDocument.Parse(json))
            {
                foreach (var candle in doc.RootElement.EnumerateArray())
                {
                    var vals = candle.EnumerateArray().ToArray();
                    var trade = new Trade
                    {
                        Id = vals[0].GetInt32().ToString(),
                        Time = DateTimeOffset.FromUnixTimeMilliseconds(vals[1].GetInt64()),
                        Amount = vals[2].GetDecimal(),
                        Price = vals[3].GetDecimal()
                    };
                    trade.Side = trade.Amount > 0 ? "buy" : "sell";
                    res.Add(trade);
                }
            }
            return res;
        }
    }
}
