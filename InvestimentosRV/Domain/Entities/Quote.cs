using Domain.Abstractions;

namespace Domain.Entities;

public class Quote : Entity
{
    public int AssetId { get; private set; }
    public decimal UnitPrice { get; private set; }
    public DateTime DateTime { get; private set; }

    public Asset Asset { get; set; } = default!;

    protected Quote() { }

    public Quote(
        int assetId,
        decimal unitPrice,
        DateTime dateTime
    )
    {
        AssetId = assetId;
        UnitPrice = unitPrice;
        DateTime = dateTime;
    }

    public static Quote Reconstitute(
        int id,
        int assetId,
        decimal unitPrice,
        DateTime dateTime,
        DateTime createdAt,
        DateTime updatedAt
    )
    {
        var quote = new Quote(assetId, unitPrice, dateTime);
        quote.Id = id;
        quote.CreatedAt = createdAt;
        quote.UpdatedAt = updatedAt;

        return quote;
    }
}
