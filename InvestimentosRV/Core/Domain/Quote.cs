namespace Core.Domain;

public class Quote : Entity
{
    public int AssetId { get; set; }
    public decimal UnitPrice { get; set; }
    public DateTime DateTime { get; set; }

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
}
