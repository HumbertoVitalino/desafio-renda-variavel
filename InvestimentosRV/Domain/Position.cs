namespace Domain;

public class Position : Entity
{
    public int UserId { get; private set; }
    public int AssetId { get; private set; }
    public int Quantity { get; private set; }
    public decimal AveragePrice { get; private set; }
    public decimal ProfitAndLoss { get; private set; }
    public User User { get; private set; } = default!;
    public Asset Asset { get; private set; } = default!;

    protected Position() { }

    public Position(
        int userId,
        int assetId,
        int quantity,
        decimal averagePrice
    )
    {
        UserId = userId;
        AssetId = assetId;
        Quantity = quantity;
        AveragePrice = averagePrice;
        ProfitAndLoss = 0;
    }

    public void UpdatePositionAfterOperation(int newTotalQuantity, decimal newAveragePrice)
    {
        if (newTotalQuantity < 0)
            throw new InvalidOperationException("The position quantity cannot be negative.");

        Quantity = newTotalQuantity;
        SetUpdatedAt();

        if (Quantity == 0)
        {
            AveragePrice = 0;
            ProfitAndLoss = 0;
            return;
        }

        AveragePrice = newAveragePrice;
    }

    public void UpdateProfitAndLossWithNewQuote(decimal newAssetUnitPrice)
    {
        SetUpdatedAt();

        if (Quantity == 0)
        {
            ProfitAndLoss = 0;
            return;
        }

        ProfitAndLoss = (Quantity * newAssetUnitPrice) - (Quantity * AveragePrice);
    }
}
