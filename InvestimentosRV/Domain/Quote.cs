namespace Domain;

public class Quote : Entity
{
    public int AssetId { get; private set; }
    public decimal UnitPrice { get; private set; }
    public DateTime DateTime { get; private set; }

    public Asset Asset { get; private set; } = default!;

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
