using ConnectorTest.Websocket.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestHQ;
using WebSocketSharp;

namespace ConnectorTest.Websocket.Fetcher
{
    internal class WebsocketTrade : IWebsocketTrade, IDisposable
    {
        private WebSocket ws;

        public event Action<Trade> NewBuyTrade;
        public event Action<Trade> NewSellTrade;

        public WebsocketTrade(string pair)
        {
            ws = new WebSocket("wss://api-pub.bitfinex.com/ws/2");
            ws.OnOpen += (s, e) =>
            {
                SubscribeTrades(pair);
            };
            ws.OnMessage += (s, e) =>
            {
                var data = e.Data;
                int a = 2;
            };
            ws.OnClose += (s, e) =>
            {
                UnsubscribeTrades(pair);
            };
            ws.Connect();
        }

        public void Dispose()
        {
            if (ws != null)
            {
                ws.Close();
            }
        }

        public async void SubscribeTrades(string pair)
        {
            string data = $"{{ \"event\": \"subscribe\", \"channel\": \"trades\", symbol: \"{pair}\"}}";
            ws.Send(data);
        }

        public void UnsubscribeTrades(string pair)
        {
            string data = $"{{ \"event\": \"unsubscribe\", \"channel\": \"trades\", symbol: \"{pair}\"}}";
            ws.Send(data);
        }
    }
}
