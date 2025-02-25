using ConnectorTest;
using ConnectorTest.Rest.Fetcher;
using ConnectorTest.Rest.Interface;
using Moq;
using Moq.Protected;
using TestHQ;

namespace IntegrationTests
{
    [TestFixture]
    public class RestTests
    {
        private ITestConnector _con;
        private Mock<CandleSeriesFetcher> _mockCandleRest;
        private Mock<NewTradesFetcher> _mockTradeRest;
        private IEnumerable<Candle> _candles;
        private IEnumerable<Trade> _trades;

        [SetUp]
        public void Setup()
        {
            _mockCandleRest = new();
            _mockTradeRest = new();
            _con = new ConnectorTest.TestConnector(_mockCandleRest.Object, _mockTradeRest.Object, null, null);
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
        public async Task TestCandlesSuccess()
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

        [Test]
        public async Task TestCandlesWrongPeriod()
        {
            //Arrange
            string pair = "tBTCUSD";
            int period = 70;
            //Act
            var act = () => _con.GetCandleSeriesAsync(pair, period);
            //Assert
            Assert.ThrowsAsync<ArgumentException>(async () => await act());
        }

        [Test]
        public async Task TestTradesSuccess()
        {
            //Arrange
            _mockTradeRest.Protected()
                           .Setup<Task<IEnumerable<Trade>>>("FetchJsonCollectionAsync", ItExpr.IsAny<string>())
                           .Returns(Task.FromResult(_trades))
                           .Verifiable();
            string pair = "tBTCUSD";
            //Act
            var res = await _con.GetNewTradesAsync(pair);
            //Assert
            Assert.AreEqual(res.Count(), _trades.Count());
            for (int i = 0; i < _trades.Count(); i++)
            {
                Assert.AreSame(_trades.ElementAt(i), res.ElementAt(i));
            }
        }
    }
}