using Core.Domain.Enums;

namespace Core.Domain;

public class Operation : Entity
{
    public int UserId { get; set; }
    public int AssetId { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public OperationType Type { get; set; }
    public decimal BrokerageFee { get; set; }
    public DateTime DateTime { get; set; }
    public User User { get; set; } = default!;
    public Asset Asset { get; set; } = default!;

    protected Operation() { }

    public Operation(
        int userId,
        int assetId,
        int quantity,
        decimal unitPrice,
        OperationType type,
        decimal brokerageFee,
        DateTime dateTime
    )
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
