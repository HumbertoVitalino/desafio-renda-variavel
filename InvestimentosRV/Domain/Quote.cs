namespace Domain;

public class Quote : Entity
{
    public int AssetId { get; private set; }
    public decimal UnitPrice { get; private set; }
    public DateTime DateTime { get; private set; }
    private Asset _asset = default!;
    public Asset Asset => _asset;

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

    public void AddAsset(Asset asset)
    {
        _asset = asset;
    }
}
