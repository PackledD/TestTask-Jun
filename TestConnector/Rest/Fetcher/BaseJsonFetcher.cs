using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace ConnectorTest.Rest.Fetcher
{
    internal class BaseJsonFetcher<T> where T : class
    {
        protected async Task<T?> FetchJsonAsync(string url)
        {
            T? res = null;
            using (var client = new HttpClient())
            {
                res = await client.GetFromJsonAsync<T>(url);
            }
            return res;
        }
    }
}
