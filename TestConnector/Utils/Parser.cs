using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using TestHQ;

namespace TestConnector.Utils
{
    internal static class Parser
    {
        public static Candle ParseCandle(JsonElement json)
        {
            var fields = json.EnumerateArray().ToArray();
            return new Candle {
                OpenTime = DateTimeOffset.FromUnixTimeMilliseconds(fields[0].GetInt64()),
                OpenPrice = fields[1].GetInt32(),
                ClosePrice = fields[2].GetInt32(),
                HighPrice = fields[3].GetInt32(),
                LowPrice = fields[4].GetInt32(),
                TotalVolume = fields[5].GetDecimal()
            };
        }

        public static IEnumerable<Candle> ParseCandleEnumerable(JsonElement json)
        {
            List<Candle> res = new List<Candle>();
            foreach (var data in json.EnumerateArray().ToArray())
            {
                res.Add(ParseCandle(data));
            }
            return res;
        }

        public static Trade ParseTrade(JsonElement json)
        {
            var fields = json.EnumerateArray().ToArray();
            var res = new Trade
            {
                Id = fields[0].GetInt32().ToString(),
                Time = DateTimeOffset.FromUnixTimeMilliseconds(fields[1].GetInt64()),
                Amount = fields[2].GetDecimal(),
                Price = fields[3].GetDecimal()
            };
            res.Side = res.Amount > 0 ? "buy" : "sell";
            return res;
        }

        public static IEnumerable<Trade> ParseTradeEnumerable(JsonElement json)
        {
            List<Trade> res = new List<Trade>();
            foreach (var data in json.EnumerateArray().ToArray())
            {
                res.Add(ParseTrade(data));
            }
            return res;
        }
    }
}
