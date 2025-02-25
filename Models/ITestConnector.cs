using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TestHQ;

namespace ConnectorTest
{
    public interface ITestConnector
    {
        #region Rest
        
        // maxCount = 100, значение по умолчанию, поскольку этот параметр необязателен при запросе
        Task<IEnumerable<Trade>> GetNewTradesAsync(string pair, int maxCount = 100);
        // from = null, значение по умолчанию, поскольку этот параметр необязателен при запросе
        Task<IEnumerable<Candle>> GetCandleSeriesAsync(string pair, int periodInSec, DateTimeOffset? from = null, DateTimeOffset? to = null, long? count = 0);

        #endregion

        #region Socket


        event Action<Trade> NewBuyTrade;
        event Action<Trade> NewSellTrade;
        void SubscribeTrades(string pair, int maxCount = 100);
        void UnsubscribeTrades(string pair);

        event Action<Candle> CandleSeriesProcessing;
        void SubscribeCandles(string pair, int periodInSec, DateTimeOffset? from = null, DateTimeOffset? to = null, long? count = 0);
        void UnsubscribeCandles(string pair);

        #endregion

    }
}
