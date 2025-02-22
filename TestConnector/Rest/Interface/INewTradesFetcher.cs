using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestHQ;

namespace TestConnector.Rest.Interface
{
    internal interface INewTradesFetcher
    {
        Task<IEnumerable<Trade>> GetNewTradesAsync(string pair, int maxCount);
    }
}
