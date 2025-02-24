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
    internal class NewTradesFetcher : BaseJsonFetcher<Trade>, INewTradesFetcher
    {
        public async Task<IEnumerable<Trade>> GetNewTradesAsync(string pair, int maxCount)
        {
            string url = $"https://api-pub.bitfinex.com/v2/trades/{pair}/hist";
            List<string> urlParams = new List<string>() { $"limit={maxCount}" };
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
            using (var doc = JsonDocument.Parse(json))
            {
                return Parser.ParseTradeEnumerable(doc.RootElement);
            }
        }
    }
}
