using Core.Commons;
using Core.Dtos;
using Core.Interfaces;
using Core.UseCase.GetTopClientsByPositionValueUseCase.Boundaries;
using MediatR;

namespace Core.UseCase.GetTopClientsByPositionValueUseCase;

public class GetTopClientsByPositionValue(
    IPositionRepository positionRepository,
    IQuoteRepository quoteRepository
) : IRequestHandler<GetTopClientsByPositionValueInput, Output>
{
    private readonly IPositionRepository _positionRepository = positionRepository;
    private readonly IQuoteRepository _quoteRepository = quoteRepository;

    public async Task<Output> Handle(GetTopClientsByPositionValueInput input, CancellationToken cancellationToken)
    {
        Output output = new();

        var allPositions = await _positionRepository.GetAllWithDetailsAsync(cancellationToken);
        var latestQuotes = await _quoteRepository.GetLatestQuotesForAllAssetsAsync(cancellationToken);

        var userPortfolioValues = allPositions
            .GroupBy(p => p.User)
            .Select(userGroup =>
            {
                var totalValue = userGroup.Sum(position =>
                {
                    latestQuotes.TryGetValue(position.AssetId, out var latestPrice);
                    return position.Quantity * latestPrice;
                });

                return new TopClientsDto
                (
                    userGroup.Key.Id,
                    userGroup.Key.Name,
                    totalValue
                );
            });

        var topClients = userPortfolioValues
            .OrderByDescending(u => u.TotalPositionValue)
            .Take(10)
            .ToList();

        output.AddResult(topClients);
        return output;
    }
}

