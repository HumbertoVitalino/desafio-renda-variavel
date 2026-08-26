using Api.Requests;
using Application.UseCases.NewOperationUseCase.Boundaries;

namespace Api.Mappers;

public static class OperationMapper
{
    public static NewOperationInput MapToInput(this NewOperationRequest request, int userId)
    {
        return new NewOperationInput(
            userId,
            request.TickerSymbol,
            request.Quantity,
            request.Type
        );
    }
}
