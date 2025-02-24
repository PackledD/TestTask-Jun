using ConnectorTest.Websocket.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using TestConnector.Websocket.Fetcher;
using TestHQ;
using WebSocketSharp;

namespace ConnectorTest.Websocket.Fetcher
{
    internal class WebsocketTrade : BaseWebsocketFetcher, IWebsocketTrade, IDisposable
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
                        var value = data.RootElement.EnumerateArray();
                        NewBuyTrade?.Invoke(new Trade { Amount = 1000, Pair = pair });
                    }
                }
            };
            Subscribe(pair, url, req, handler);
        }

        public void UnsubscribeTrades(string pair)
        {
            Unsubscribe(pair);
        }
    }
}
