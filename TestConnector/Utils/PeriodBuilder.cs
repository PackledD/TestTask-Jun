using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectorTest.Utils
{
    internal static class PeriodBuilder
    {
        public static string? FromSec(int sec)
        {
            switch (sec)
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
