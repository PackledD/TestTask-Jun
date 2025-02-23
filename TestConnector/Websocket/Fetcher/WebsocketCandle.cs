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
    internal class WebsocketCandle : IWebsocketCandle
    {
        private WebSocket ws;

        public event Action<Candle> CandleSeriesProcessing;

        public WebsocketCandle(string pair, int periodInSec, DateTimeOffset? from, DateTimeOffset? to, long? count)
        {
            ws = new WebSocket("wss://api-pub.bitfinex.com/ws/2");
            ws.OnOpen += (s, e) =>
            {
                SubscribeCandles(pair, periodInSec, from, to, count);
            };
            ws.OnMessage += (s, e) =>
            {
                var data = e.Data;
                int a = 2;
            };
            ws.OnClose += (s, e) =>
            {
                UnsubscribeCandles(pair);
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

        public void SubscribeCandles(string pair, int periodInSec, DateTimeOffset? from, DateTimeOffset? to, long? count)
        {
            string data = $"{{ \"event\": \"subscribe\", \"channel\": \"trades\", symbol: \"{pair}\"}}";
            ws.Send(data);
        }

        public void UnsubscribeCandles(string pair)
        {
            string data = $"{{ \"event\": \"unsubscribe\", \"channel\": \"trades\", symbol: \"{pair}\"}}";
            ws.Send(data);
        }
    }
}
