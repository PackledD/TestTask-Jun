using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConnectorTest.Rest.Interface;
using ConnectorTest.Utils;
using TestHQ;

namespace ConnectorTest.Rest.Fetcher
{
    internal class NewTradesFetcher : BaseJsonFetcher<IEnumerable<Trade>>, INewTradesFetcher
    {
        public async Task<IEnumerable<Trade>> GetNewTradesAsync(string pair, int maxCount)
        {
            string url = $"https://api-pub.bitfinex.com/v2/trades/{pair}/hist";
            List<string> urlParams = new() { $"limit={maxCount}" };
            var res = await FetchJsonAsync(UrlBuilder.Build(url, urlParams));
            if (res == null)
            {
                throw new HttpRequestException("Can't fetch data");
            }
            return res;
        }
    }
}
