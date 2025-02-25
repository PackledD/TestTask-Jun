using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestHQ;

namespace ConnectorTest.Websocket.Interface
{
    public interface IWebsocketTrade
    {
        event Action<Trade> NewBuyTrade;
        event Action<Trade> NewSellTrade;
        void SubscribeTrades(string pair, int maxCount);
        void UnsubscribeTrades(string pair);
    }
}
