using Domain.Enums;

namespace Domain;

public class Asset : Entity
{
    public string TickerSymbol { get; private set; } = string.Empty;
    public string Name { get; private set; } = string.Empty;
    public AssetRisk Risk { get; private set; }
    public ICollection<Quote> Quotes { get; private set; } = [];
    public ICollection<Operation> Operations { get; private set; } = [];
    public ICollection<Position> Positions { get; private set; } = [];

    protected Asset() { }

    public Asset(
        string tickerSymbol,
        string name,
        AssetRisk risk
    )
    {
        TickerSymbol = tickerSymbol.ToUpperInvariant();
        Name = name;
        Risk = risk;
    }
}