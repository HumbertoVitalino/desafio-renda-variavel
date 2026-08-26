namespace Application.UseCases.GetUserAssetPositionUseCase.Boundaries;

public sealed record GetUserAssetPositionInput(
    int UserId,
    string TickerSymbol
);
