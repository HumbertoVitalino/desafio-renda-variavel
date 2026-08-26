namespace Application.UseCases.GetLatestQuoteUseCase.Boundaries;

public sealed record GetLatestQuoteInput(
    string TickerSymbol
);
