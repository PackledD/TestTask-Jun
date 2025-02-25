using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConnectorTest.Rest.Fetcher;
using ConnectorTest.Rest.Interface;
using ConnectorTest.Websocket.Fetcher;
using ConnectorTest.Websocket.Interface;
using TestHQ;

namespace ConnectorTest
{
    public class TestConnector : ITestConnector
    {
        private ICandleSeriesFetcher _candlesRest;
        private INewTradesFetcher _tradesRest;
        private IWebsocketCandle _candleWs;
        private IWebsocketTrade _tradeWs;

        public event Action<Trade> NewBuyTrade
        {
            add
            {
                _tradeWs.NewBuyTrade += value;
            }
            remove
            {
                _tradeWs.NewBuyTrade -= value;
            }
        }
        public event Action<Trade> NewSellTrade
        {
            add
            {
                _tradeWs.NewSellTrade += value;
            }
            remove
            {
                _tradeWs.NewSellTrade -= value;
            }
        }
        public event Action<Candle> CandleSeriesProcessing
        {
            add
            {
                _candleWs.CandleSeriesProcessing += value;
            }
            remove
            {
                _candleWs.CandleSeriesProcessing -= value;
            }
        }

        public TestConnector()
        {
            _candlesRest = new CandleSeriesFetcher();
            _tradesRest = new NewTradesFetcher();
            _candleWs = new WebsocketCandle();
            _tradeWs = new WebsocketTrade();
        }

        public TestConnector(ICandleSeriesFetcher candlesRest, INewTradesFetcher tradesRest, IWebsocketCandle candleWs, IWebsocketTrade tradeWs)
        {
            _candlesRest = candlesRest;
            _tradesRest = tradesRest;
            _candleWs = candleWs;
            _tradeWs = tradeWs;
        }

        public Task<IEnumerable<Candle>> GetCandleSeriesAsync(string pair, int periodInSec, DateTimeOffset? from, DateTimeOffset? to = null, long? count = 0)
        {
            return _candlesRest.GetCandleSeriesAsync(pair, periodInSec, from, to, count);
        }

        public Task<IEnumerable<Trade>> GetNewTradesAsync(string pair, int maxCount)
        {
            return _tradesRest.GetNewTradesAsync(pair, maxCount);
        }

        public void SubscribeCandles(string pair, int periodInSec, DateTimeOffset? from = null, DateTimeOffset? to = null, long? count = 0)
        {
            _candleWs.SubscribeCandles(pair, periodInSec, from, to, count);
        }

        public void SubscribeTrades(string pair, int maxCount)
        {
            _tradeWs.SubscribeTrades(pair, maxCount);
        }

        public void UnsubscribeCandles(string pair)
        {
            _candleWs.UnsubscribeCandles(pair);
        }

        public void UnsubscribeTrades(string pair)
        {
            _tradeWs.UnsubscribeTrades(pair);
        }
    }
}
