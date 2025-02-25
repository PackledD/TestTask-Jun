using ConnectorTest.Rest.Fetcher;
using ConnectorTest;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestHQ;
using ConnectorTest.Websocket.Fetcher;
using Moq.Protected;

namespace IntegrationTests
{
    [TestFixture]
    public class WebsocketTests
    {
        private ITestConnector _con;
        private Mock<WebsocketCandle> _mockCandleWs;
        private Mock<WebsocketTrade> _mockTradeWs;
        private IEnumerable<Candle> _candles;
        private IEnumerable<Trade> _trades;

        [SetUp]
        public void Setup()
        {
            _mockCandleWs = new();
            _mockTradeWs = new();
            _con = new ConnectorTest.TestConnector(null, null, _mockCandleWs.Object, _mockTradeWs.Object);
            _candles = new List<Candle>()
            {
                new Candle { ClosePrice = 100, OpenPrice = 100, HighPrice = 200,
                             LowPrice = 50, OpenTime = DateTimeOffset.Now, TotalVolume = 1600 },
                new Candle { ClosePrice = 120, OpenPrice = 120, HighPrice = 250,
                             LowPrice = 75, OpenTime = DateTimeOffset.UtcNow, TotalVolume = 1800 },
                new Candle { ClosePrice = 160, OpenPrice = 110, HighPrice = 160,
                             LowPrice = 90, OpenTime = DateTimeOffset.Now, TotalVolume = 2400 }
            };
            _trades = new List<Trade>()
            {
                new Trade { Id = "111", Amount = 2, Price = 100, Side = "buy", Time = DateTimeOffset.UtcNow },
                new Trade { Id = "222", Amount = 3, Price = 200, Side = "buy", Time = DateTimeOffset.UtcNow },
                new Trade { Id = "333", Amount = 4, Price = -300, Side = "sell", Time = DateTimeOffset.UtcNow }
            };

        }

        [Test]
        public async Task TestCandles()
        {
            //Arrange
            _mockCandleRest.Protected()
                           .Setup<Task<IEnumerable<Candle>>>("FetchJsonCollectionAsync", ItExpr.IsAny<string>())
                           .Returns(Task.FromResult(_candles))
                           .Verifiable();
            string pair = "tBTCUSD";
            int period = 60;
            //Act
            var res = await _con.GetCandleSeriesAsync(pair, period);
            //Assert
            Assert.AreEqual(res.Count(), _candles.Count());
            for (int i = 0; i < _candles.Count(); i++)
            {
                Assert.AreSame(_candles.ElementAt(i), res.ElementAt(i));
            }
        }
    }
}
