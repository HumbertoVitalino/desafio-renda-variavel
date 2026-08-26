namespace Infra.Repositories.Models;

public sealed class PositionModel : Model
{
    public int UserId { get; private set; }
    public int AssetId { get; private set; }
    public int Quantity { get; private set; }
    public decimal AveragePrice { get; private set; }
    public decimal ProfitAndLoss { get; private set; }

    public UserModel User { get; private set; } = default!;
    public AssetModel Asset { get; private set; } = default!;

    private PositionModel() { }

    public PositionModel(
        int id,
        int userId,
        int assetId,
        int quantity,
        decimal averagePrice,
        decimal profitAndLoss,
        DateTime createdAt,
        DateTime updatedAt
    ) : base(id, createdAt, updatedAt)
    {
        UserId = userId;
        AssetId = assetId;
        Quantity = quantity;
        AveragePrice = averagePrice;
        ProfitAndLoss = profitAndLoss;
    }
}
