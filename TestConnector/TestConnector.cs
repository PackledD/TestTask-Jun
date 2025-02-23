﻿using System;
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
        public event Action<Trade> NewBuyTrade;
        public event Action<Trade> NewSellTrade;
        public event Action<Candle> CandleSeriesProcessing;

        private ICandleSeriesFetcher _candlesRest;
        private INewTradesFetcher _tradesRest;
        private IWebsocketCandle _candleWs;
        private IWebsocketTrade _tradeWs;

        public TestConnector()
        {
            _candlesRest = new CandleSeriesFetcher();
            _tradesRest = new NewTradesFetcher();
            //_candleWs = new WebsocketCandle();
            _tradeWs = new WebsocketTrade("tBTCUSD");
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

        public void SubscribeTrades(string pair)
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
