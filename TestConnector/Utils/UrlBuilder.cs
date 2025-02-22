using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestConnector.Utils
{
    internal static class UrlBuilder
    {
        public static string Build(string urlBase, IEnumerable<string> urlParams)
        {
            string res = urlBase;
            foreach (var param in urlParams)
            {
                res += $"{param}&";
            }
            return res.TrimEnd('&');
        }
    }
}
