using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TestHQ;

namespace ConnectorTest.Rest.Interface
{
    public interface ICandleSeriesFetcher
    {
        Task<IEnumerable<Candle>> GetCandleSeriesAsync(string pair, int periodInSec, DateTimeOffset? from, DateTimeOffset? to, long? count);
    }
}
