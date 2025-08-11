using Core.Commons;
using Domain.Enums;
using MediatR;

namespace Core.UseCase.NewOperationUseCase.Boundaries;

public sealed record NewOperationInput(
    int UserId,
    string TickerSymbol,
    int Quantity,
    OperationType Type
) : IRequest<Output>;
