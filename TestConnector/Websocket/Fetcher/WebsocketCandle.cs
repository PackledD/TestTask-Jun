using ConnectorTest.Utils;
using ConnectorTest.Websocket.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using TestConnector.Utils;
using TestConnector.Websocket.Fetcher;
using TestHQ;
using WebSocketSharp;

namespace ConnectorTest.Websocket.Fetcher
{
    public class WebsocketCandle : BaseWebsocketFetcher, IWebsocketCandle, IDisposable
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
                        bool hasCounter = (count != null);
                        foreach (var cand in ParseCandles(data.RootElement))
                        {
                            cand.Pair = pair;
                            if ((from == null || from < cand.OpenTime) && (to == null || cand.OpenTime < to))
                            {
                                CandleSeriesProcessing?.Invoke(cand);
                            }
                            if (hasCounter)
                            {
                                count--;
                                if (count == 0)
                                {
                                    return;
                                }
                            }
                        }
                    }
                }
            };
            Subscribe(pair, url, req, handler);
        }

        public void UnsubscribeCandles(string pair)
        {
            Unsubscribe(pair);
        }

        private IEnumerable<Candle> ParseCandles(JsonElement el)
        {
            var arr = el.EnumerateArray().ToArray();
            if (arr[1].ValueKind == JsonValueKind.Array)
            {
                var prevArr = arr[1];
                arr = prevArr.EnumerateArray().ToArray();
                if (arr.Count() == 0)
                {
                    return new List<Candle>();
                }
                else if (arr[0].ValueKind == JsonValueKind.Array)
                {
                    return Parser.ParseCandleEnumerable(prevArr);
                }
                else
                {
                    return new List<Candle> { Parser.ParseCandle(prevArr) };
                }
            }
            return new List<Candle>();
        }
    }
}
