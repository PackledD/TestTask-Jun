using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestHQ;

namespace ConnectorTest.Websocket.Interface
{
    public interface IWebsocketCandle
    {
        event Action<Candle> CandleSeriesProcessing;
        void SubscribeCandles(string pair, int periodInSec, DateTimeOffset? from, DateTimeOffset? to, long? count);
        void UnsubscribeCandles(string pair);
    }
}
