namespace Core.Domain;

public class Position : Entity
{
    public int UserId { get; set; }
    public int AssetId { get; set; }
    public int Quantity { get; set; }
    public decimal AveragePrice { get; set; }
    public decimal ProfitAndLoss { get; set; } = 0;
    public User User { get; set; } = default!;
    public Asset Asset { get; set; } = default!;

    protected Position() { }

    public Position(
        int userId,
        int assetId,
        int quantity,
        decimal averagePrice,
        decimal profitAndLoss
    )
    {
        UserId = userId;
        AssetId = assetId;
        Quantity = quantity;
        AveragePrice = averagePrice;
        ProfitAndLoss = profitAndLoss;
    }
}
