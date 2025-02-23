using System.Collections.Generic;
using System.Threading.Tasks;
using TestHQ;

namespace ConnectorTest.Rest.Interface
{
    internal interface INewTradesFetcher
    {
        Task<IEnumerable<Trade>> GetNewTradesAsync(string pair, int maxCount);
    }
}
