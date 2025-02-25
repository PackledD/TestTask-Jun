using ConnectorTest;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using TestHQ;
using TestWPF.Models;

namespace TestWPF.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private ITestConnector _con;
        private string _pairNameTrades;
        private int _maxCount = 100;
        private string _pairNameCandles;
        private int _periodInSec;
        private DateTimeOffset? _from;
        private DateTimeOffset? _to;
        private long? _count;

        public string PairNameTrades
        {
            get { return _pairNameTrades; }
            set
            {
                _pairNameTrades = value;
                OnPropertyChanged();
            }
        }
        public int MaxCount
        {
            get { return _maxCount; }
            set
            {
                _maxCount = value;
                OnPropertyChanged();
            }
        }
        public string PairNameCandles
        {
            get { return _pairNameCandles; }
            set
            {
                _pairNameCandles = value;
                OnPropertyChanged();
            }
        }
        public int PeriodInSec
        {
            get { return _periodInSec; }
            set
            {
                _periodInSec = value;
                OnPropertyChanged();
            }
        }
        public DateTimeOffset? From
        {
            get { return _from; }
            set
            {
                _from = value;
                OnPropertyChanged();
            }
        }
        public DateTimeOffset? To
        {
            get { return _to; }
            set
            {
                _to = value;
                OnPropertyChanged();
            }
        }
        public long? Count
        {
            get { return _count; }
            set
            {
                _count = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Trade> Trades { get; set; } = new ObservableCollection<Trade>();
        public ObservableCollection<Candle> Candles { get; set; } = new ObservableCollection<Candle>();

        public ICommand AddTradeSubscription { get; }
        public ICommand RemoveTradeSubscription { get; }
        public ICommand AddCandleSubscription { get; }
        public ICommand RemoveCandleSubscription { get; }

        public MainWindowViewModel()
        {
            _con = new ConnectorTest.TestConnector();
            AddTradeSubscription = new Cmd(AddTrade);
            RemoveTradeSubscription = new Cmd(RemoveTrade);
            AddCandleSubscription = new Cmd(AddCandle);
            RemoveCandleSubscription = new Cmd(RemoveCandle);
        }

        private void AddTrade()
        {
            if (!string.IsNullOrEmpty(PairNameTrades))
            {
                try
                {
                    _con.SubscribeTrades(PairNameTrades, MaxCount);
                    _con.NewBuyTrade += Trade => Application.Current.Dispatcher.Invoke(
                        () => Trades.Add(Trade)
                    );
                    _con.NewSellTrade += Trade => Application.Current.Dispatcher.Invoke(
                        () => Trades.Add(Trade)
                    );
                }
                catch
                {
                    MessageBox.Show("Can't subscribe on trade");
                }
            }
        }

        private void RemoveTrade()
        {
            if (!string.IsNullOrEmpty(PairNameTrades))
            {
                _con.UnsubscribeTrades(PairNameTrades);
            }
        }

        private void AddCandle()
        {
            if (!string.IsNullOrEmpty(PairNameCandles))
            {
                try
                {
                    _con.SubscribeCandles(PairNameCandles, PeriodInSec, From, To, Count);
                    _con.CandleSeriesProcessing += Candle => Application.Current.Dispatcher.Invoke(
                        () => Candles.Add(Candle)
                    );
                }
                catch
                {
                    MessageBox.Show("Can't subscribe on candle");
                }
            }
        }

        private void RemoveCandle()
        {
            if (!string.IsNullOrEmpty(PairNameCandles))
            {
                _con.UnsubscribeTrades(PairNameCandles);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class Cmd : ICommand
    {
        private readonly Action _execute;
        private readonly Func<bool> _canExecute;

        public Cmd(Action execute, Func<bool> canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter) => _canExecute == null || _canExecute();

        public void Execute(object parameter) => _execute();

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
    }
}