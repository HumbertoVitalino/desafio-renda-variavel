using Core.Domain;
using Core.Dtos;
using Core.UseCase.NewOperationUseCase.Boundaries;

namespace Core.Mappers;

public static class PositionMapper
{
    public static Position MapPositionToDomain(this NewOperationInput input, int assetId, decimal executionPrice)
    {
        return new Position(
            input.UserId,
            assetId,
            input.Quantity,
            executionPrice
        );
    }

    public static PositionDto MapToDto(this Position position)
    {
        return new PositionDto(
            position.Asset.TickerSymbol,
            position.Asset.Name,
            position.Quantity,
            position.AveragePrice,
            position.ProfitAndLoss
        );
    }

    public static IEnumerable<PositionDto> MapToDto(this IEnumerable<Position> positions)
    {
        return positions.Select(position => position.MapToDto());
    }
}
