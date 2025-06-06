namespace Core.Domain;

public class Asset : Entity
{
    public string TickerSymbol { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public ICollection<Quote> Quotes { get; set; } = [];
    public ICollection<Operation> Operations { get; set; } = [];
    public ICollection<Position> Positions { get; set; } = [];

    protected Asset() { }

    public Asset(string tickerSymbol, string name)
    {
        TickerSymbol = tickerSymbol.ToUpperInvariant();
        Name = name;
    }
}
