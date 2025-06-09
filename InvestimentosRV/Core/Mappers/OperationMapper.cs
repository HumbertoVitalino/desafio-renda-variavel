using Core.Domain;
using Core.Dtos;
using Core.UseCase.NewOperationUseCase.Boundaries;

namespace Core.Mappers;

public static class OperationMapper
{
    public static Operation MapToDomain(this NewOperationInput input, int assetId, decimal executionPrice, decimal brokerageFee)
    {
        return new Operation(
            input.UserId,
            assetId,
            input.Quantity,
            executionPrice,
            input.Type,
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
