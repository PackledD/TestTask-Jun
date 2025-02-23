using ConnectorTest.Websocket.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using TestHQ;
using WebSocketSharp;

namespace ConnectorTest.Websocket.Fetcher
{
    public class WebsocketTrade : IWebsocketTrade, IDisposable
    {
        private Dictionary<string, WebSocket> sockets = new Dictionary<string, WebSocket>();

        public event Action<Trade> NewBuyTrade;
        public event Action<Trade> NewSellTrade;

        public void SubscribeTrades(string pair, int maxCount)
        {
            if (sockets.ContainsKey(pair))
            {
                return;
            }
            sockets[pair] = new WebSocket("wss://api-pub.bitfinex.com/ws/2");
            sockets[pair].Connect();
            sockets[pair].OnMessage += (sender, e) =>
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
            string req = $"{{ \"event\": \"subscribe\", \"channel\": \"trades\", \"symbol\": \"{pair}\"}}";
            sockets[pair].Send(req);
        }

        public void UnsubscribeTrades(string pair)
        {
            if (sockets.ContainsKey(pair))
            {
                sockets[pair].Close();
                sockets.Remove(pair);
            }
        }

        public void Dispose()
        {
            foreach (var ws in sockets.Values)
            {
                ws.Close();
            }
        }
    }
}
