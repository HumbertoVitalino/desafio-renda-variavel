using Core.Domain;
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
}
