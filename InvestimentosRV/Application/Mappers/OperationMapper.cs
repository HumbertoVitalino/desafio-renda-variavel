using Application.DTOs;
using Domain.Entities;
using Domain.Enums;

namespace Application.Mappers;

public static class OperationMapper
{
    public static Operation MapToDomain(int userId, int assetId, int quantity, decimal executionPrice, OperationType type, decimal brokerageFee)
    {
        return new Operation(
            userId,
            assetId,
            quantity,
            executionPrice,
            type,
            brokerageFee,
            DateTime.UtcNow
        );
    }

    public static OperationDto MapToDto(this Operation operation)
    {
        return new OperationDto(
            operation.Id,
            operation.Asset.TickerSymbol,
            operation.Type,
            operation.Quantity,
            operation.UnitPrice,
            operation.BrokerageFee,
            operation.DateTime
        );
    }
}
