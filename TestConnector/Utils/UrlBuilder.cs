using System.Collections.Generic;

namespace ConnectorTest.Utils
{
    internal static class UrlBuilder
    {
        public static string Build(string urlDomain, IEnumerable<string> urlParams)
        {
            string res = urlDomain + "?";
            foreach (var param in urlParams)
            {
                res += $"{param}&";
            }
            return res.TrimEnd('&');
        }
    }
}
