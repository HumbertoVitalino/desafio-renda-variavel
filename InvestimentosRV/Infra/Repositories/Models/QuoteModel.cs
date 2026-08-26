namespace Infra.Repositories.Models;

public sealed class QuoteModel : Model
{
    public int AssetId { get; private set; }
    public decimal UnitPrice { get; private set; }
    public DateTime DateTime { get; private set; }

    public AssetModel Asset { get; private set; } = default!;

    private QuoteModel() { }

    public QuoteModel(
        int id,
        int assetId,
        decimal unitPrice,
        DateTime dateTime,
        DateTime createdAt,
        DateTime updatedAt
    ) : base(id, createdAt, updatedAt)
    {
        AssetId = assetId;
        UnitPrice = unitPrice;
        DateTime = dateTime;
    }
}
