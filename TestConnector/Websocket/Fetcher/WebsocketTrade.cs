using ConnectorTest.Websocket.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.Json;
using TestConnector.Utils;
using TestConnector.Websocket.Fetcher;
using TestHQ;
using WebSocketSharp;

namespace ConnectorTest.Websocket.Fetcher
{
    public class WebsocketTrade : BaseWebsocketFetcher, IWebsocketTrade, IDisposable
    {
        public event Action<Trade> NewBuyTrade;
        public event Action<Trade> NewSellTrade;

        public void SubscribeTrades(string pair, int maxCount)
        {
            string url = "wss://api-pub.bitfinex.com/ws/2";
            string req = $"{{ \"event\": \"subscribe\", \"channel\": \"trades\", \"symbol\": \"{pair}\"}}";
            EventHandler<MessageEventArgs> handler = (sender, e) =>
            {
                using (var data = JsonDocument.Parse(e.Data))
                {
                    if (data.RootElement.ValueKind == JsonValueKind.Array)
                    {
                        foreach (var trade in ParseTrades(data.RootElement))
                        {
                            maxCount--;
                            trade.Pair = pair;
                            if (trade.Side == "buy")
                            {
                                NewBuyTrade?.Invoke(trade);
                            }
                            else
                            {
                                NewSellTrade?.Invoke(trade);
                            }
                            if (maxCount == 0)
                            {
                                return;
                            }
                        }
                    }
                }
            };
            Subscribe(pair, url, req, handler);
        }

        public void UnsubscribeTrades(string pair)
        {
            Unsubscribe(pair);
        }

        private IEnumerable<Trade> ParseTrades(JsonElement el)
        {
            var arr = el.EnumerateArray().ToArray();
            if (arr[1].ValueKind == JsonValueKind.Array)
            {
                var prevArr = arr[1];
                arr = prevArr.EnumerateArray().ToArray();
                if (arr.Count() == 0)
                {
                    return new List<Trade>();
                }
                else if (arr[0].ValueKind == JsonValueKind.Array)
                {
                    return Parser.ParseTradeEnumerable(prevArr);
                }
                else
                {
                    return new List<Trade> { Parser.ParseTrade(prevArr) };
                }
            }
            else if (arr[1].ValueKind == JsonValueKind.String)
            {
                if (arr[1].GetString() != "hb")
                {
                    return new List<Trade> { Parser.ParseTrade(arr[2]) };
                }
            }
            return new List<Trade>();
        }
    }
}
