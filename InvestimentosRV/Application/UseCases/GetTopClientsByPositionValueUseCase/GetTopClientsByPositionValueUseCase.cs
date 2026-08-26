using Application.Commons;
using Application.DTOs;
using Application.Interfaces.Repositories;
using Application.Interfaces.UseCases;
using Application.UseCases.GetTopClientsByPositionValueUseCase.Boundaries;

namespace Application.UseCases.GetTopClientsByPositionValueUseCase;

public class GetTopClientsByPositionValueUseCase(
    IPositionRepository positionRepository,
    IQuoteRepository quoteRepository
) : IGetTopClientsByPositionValueUseCase
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
