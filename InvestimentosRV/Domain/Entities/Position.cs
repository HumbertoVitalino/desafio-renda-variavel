using Domain.Abstractions;
using Domain.Errors;

namespace Domain.Entities;

public class Position : Entity
{
    public int UserId { get; private set; }
    public int AssetId { get; private set; }
    public int Quantity { get; private set; }
    public decimal AveragePrice { get; private set; }
    public decimal ProfitAndLoss { get; private set; }
    public User User { get; set; } = default!;
    public Asset Asset { get; set; } = default!;

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

    public void ApplyBuy(int quantity, decimal unitPrice)
    {
        var totalValueOld = Quantity * AveragePrice;
        var totalValueNew = quantity * unitPrice;
        var totalQuantity = Quantity + quantity;
        var newAveragePrice = (totalValueOld + totalValueNew) / totalQuantity;

        UpdatePositionAfterOperation(totalQuantity, newAveragePrice);
    }

    public void ApplySell(int quantity)
    {
        if (quantity > Quantity)
            throw new DomainException(PositionErrors.InsufficientQuantity);

        UpdatePositionAfterOperation(Quantity - quantity, AveragePrice);
    }

    private void UpdatePositionAfterOperation(int newTotalQuantity, decimal newAveragePrice)
    {
        if (newTotalQuantity < 0)
            throw new DomainException(PositionErrors.NegativeQuantity);

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
