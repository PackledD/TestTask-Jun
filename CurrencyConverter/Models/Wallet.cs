namespace CurrencyConverter.Models
{
    public class Wallet
    {
        public Dictionary<string, double> Balances { get; set; } = new Dictionary<string, double>();
        public IEnumerable<string> Currencies { get => Balances.Keys; }

    }
}
