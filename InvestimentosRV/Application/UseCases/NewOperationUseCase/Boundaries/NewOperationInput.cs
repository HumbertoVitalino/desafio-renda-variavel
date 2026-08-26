using Domain.Enums;

namespace Application.UseCases.NewOperationUseCase.Boundaries;

public sealed record NewOperationInput(
    int UserId,
    string TickerSymbol,
    int Quantity,
    OperationType Type
);
