using ConnectorTest.Utils;
using ConnectorTest.Websocket.Interface;
using System;
using System.Collections.Generic;
using System.Text.Json;
using TestConnector.Websocket.Fetcher;
using TestHQ;
using WebSocketSharp;

namespace ConnectorTest.Websocket.Fetcher
{
    internal class WebsocketCandle : BaseWebsocketFetcher, IWebsocketCandle, IDisposable
    {
        public event Action<Candle> CandleSeriesProcessing;

        public void SubscribeCandles(string pair, int periodInSec, DateTimeOffset? from, DateTimeOffset? to, long? count)
        {
            string period = PeriodBuilder.FromSec(periodInSec);
            string url = "wss://api-pub.bitfinex.com/ws/2";
            string req = $"{{ \"event\": \"subscribe\", \"channel\": \"candles\", \"key\": \"trade:{period}:{pair}\"}}";
            EventHandler<MessageEventArgs> handler = (sender, e) =>
            {
                using (var data = JsonDocument.Parse(e.Data))
                {
                    if (data.RootElement.ValueKind == JsonValueKind.Array)
                    {
                        var value = data.RootElement.EnumerateArray();
                        CandleSeriesProcessing?.Invoke(new Candle { Pair = pair });
                    }
                }
            };
            Subscribe(pair, url, req, handler);
        }

        public void UnsubscribeCandles(string pair)
        {
            Unsubscribe(pair);
        }
    }
}
