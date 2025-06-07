using Core.Commons;
using MediatR;

namespace Core.UseCase.GetLatestQuoteUseCase.Boundaries;

public sealed record GetLatestQuoteInput(
    string TickerSymbol
) : IRequest<Output>;
