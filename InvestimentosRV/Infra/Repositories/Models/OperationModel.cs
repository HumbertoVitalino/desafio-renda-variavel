using Domain.Enums;

namespace Infra.Repositories.Models;

public sealed class OperationModel : Model
{
    public int UserId { get; private set; }
    public int AssetId { get; private set; }
    public int Quantity { get; private set; }
    public decimal UnitPrice { get; private set; }
    public OperationType Type { get; private set; }
    public decimal BrokerageFee { get; private set; }
    public DateTime DateTime { get; private set; }

    public UserModel User { get; private set; } = default!;
    public AssetModel Asset { get; private set; } = default!;

    private OperationModel() { }

    public OperationModel(
        int id,
        int userId,
        int assetId,
        int quantity,
        decimal unitPrice,
        OperationType type,
        decimal brokerageFee,
        DateTime dateTime,
        DateTime createdAt,
        DateTime updatedAt
    ) : base(id, createdAt, updatedAt)
    {
        UserId = userId;
        AssetId = assetId;
        Quantity = quantity;
        UnitPrice = unitPrice;
        Type = type;
        BrokerageFee = brokerageFee;
        DateTime = dateTime;
    }
}
