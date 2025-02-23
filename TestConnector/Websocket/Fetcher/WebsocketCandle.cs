using ConnectorTest.Websocket.Interface;
using System;
using TestHQ;
using WebSocketSharp;

namespace ConnectorTest.Websocket.Fetcher
{
    internal class WebsocketCandle : IWebsocketCandle
    {
        private WebSocket ws;

        public event Action<Candle> CandleSeriesProcessing;

        public WebsocketCandle()
        {
            ws = new WebSocket("wss://api-pub.bitfinex.com/ws/2");
        }

        public void SubscribeCandles(string pair, int periodInSec, DateTimeOffset? from, DateTimeOffset? to, long? count)
        {
            ws.OnMessage += (sender, e) =>
            {
                var d = e.Data;
            };
            ws.Connect();
            string data = $"{{ \"event\": \"subscribe\", \"channel\": \"trades\", \"symbol\": \"{pair}\"}}";
            ws.Send(data);
        }

        public void UnsubscribeCandles(string pair)
        {
            string data = $"{{ \"event\": \"unsubscribe\", \"channel\": \"trades\", \"symbol\": \"{pair}\"}}";
            ws.Send(data);
            ws.Close();
        }
    }
}
