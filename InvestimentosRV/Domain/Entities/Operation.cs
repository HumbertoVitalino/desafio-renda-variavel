using Domain.Abstractions;
using Domain.Enums;

namespace Domain.Entities;

public class Operation : Entity
{
    public int UserId { get; private set; }
    public int AssetId { get; private set; }
    public int Quantity { get; private set; }
    public decimal UnitPrice { get; private set; }
    public OperationType Type { get; private set; }
    public decimal BrokerageFee { get; private set; }
    public DateTime DateTime { get; private set; }
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

    public static Operation Reconstitute(
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
    )
    {
        var operation = new Operation(userId, assetId, quantity, unitPrice, type, brokerageFee, dateTime);
        operation.Id = id;
        operation.CreatedAt = createdAt;
        operation.UpdatedAt = updatedAt;

        return operation;
    }
}
