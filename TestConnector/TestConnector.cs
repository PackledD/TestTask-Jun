using ConnectorTest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestConnector.Rest.Fetcher;
using TestConnector.Rest.Interface;
using TestHQ;

namespace TestConnector
{
    public class TestConnector : ITestConnector
    {
        public event Action<Trade> NewBuyTrade;
        public event Action<Trade> NewSellTrade;
        public event Action<Candle> CandleSeriesProcessing;

        private ICandleSeriesFetcher _candlesRest;
        private INewTradesFetcher _tradesRest;

        public TestConnector()
        {
            _candlesRest = new CandleSeriesFetcher();
            _tradesRest = new NewTradesFetcher();
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
            throw new NotImplementedException();
        }

        public void SubscribeTrades(string pair, int maxCount = 100)
        {
            throw new NotImplementedException();
        }

        public void UnsubscribeCandles(string pair)
        {
            throw new NotImplementedException();
        }

        public void UnsubscribeTrades(string pair)
        {
            throw new NotImplementedException();
        }
    }
}
