using Domain.Enums;

namespace Infra.Repositories.Models;

public sealed class AssetModel : Model
{
    public string TickerSymbol { get; private set; } = default!;
    public string Name { get; private set; } = default!;
    public AssetRisk Risk { get; private set; }

    public ICollection<QuoteModel> Quotes { get; private set; } = [];
    public ICollection<OperationModel> Operations { get; private set; } = [];
    public ICollection<PositionModel> Positions { get; private set; } = [];

    private AssetModel() { }

    public AssetModel(
        int id,
        string tickerSymbol,
        string name,
        AssetRisk risk,
        DateTime createdAt,
        DateTime updatedAt
    ) : base(id, createdAt, updatedAt)
    {
        TickerSymbol = tickerSymbol;
        Name = name;
        Risk = risk;
    }
}
