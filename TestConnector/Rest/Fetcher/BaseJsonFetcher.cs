using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace ConnectorTest.Rest.Fetcher
{
    public abstract class BaseJsonFetcher<T> where T : class
    {
        protected virtual async Task<IEnumerable<T>> FetchJsonCollectionAsync(string url)
        {
            IEnumerable<T> res = null;
            using (var client = new HttpClient())
            {
                var resp = await client.GetAsync(url);
                if (resp.IsSuccessStatusCode)
                {
                    res = ParseEntityCollection(await resp.Content.ReadAsStringAsync());
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
