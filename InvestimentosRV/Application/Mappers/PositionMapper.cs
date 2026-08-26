using Application.DTOs;
using Domain.Entities;

namespace Application.Mappers;

public static class PositionMapper
{
    public static Position MapPositionToDomain(int userId, int assetId, int quantity, decimal executionPrice)
    {
        return new Position(
            userId,
            assetId,
            quantity,
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
