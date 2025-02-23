using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace ConnectorTest.Rest.Fetcher
{
    internal abstract class BaseJsonFetcher<T> where T : class
    {
        protected async Task<IEnumerable<T>> FetchJsonCollectionAsync(string url)
        {
            IEnumerable<T> res = null;
            using (var client = new HttpClient())
            {
                var resp = await client.GetAsync(url);
                if (resp.IsSuccessStatusCode)
                {
                    res = ParseEntityCollection(resp.Content.ToString());
                }
            }
            return res;
        }

        protected virtual IEnumerable<T> ParseEntityCollection(string json)
        {
            throw new NotImplementedException();
        }
    }
}
