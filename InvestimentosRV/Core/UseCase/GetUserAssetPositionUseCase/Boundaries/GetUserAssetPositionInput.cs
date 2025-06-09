using Core.Commons;
using MediatR;

namespace Core.UseCase.GetUserAssetPositionUseCase.Boundaries;

public sealed record GetUserAssetPositionInput(
    int UserId,
    string TickerSymbol
) : IRequest<Output>;
