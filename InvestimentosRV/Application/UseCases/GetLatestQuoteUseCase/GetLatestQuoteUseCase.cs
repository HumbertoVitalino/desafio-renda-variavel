using Application.Commons;
using Application.Interfaces.Repositories;
using Application.Interfaces.UseCases;
using Application.Mappers;
using Application.UseCases.GetLatestQuoteUseCase.Boundaries;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.GetLatestQuoteUseCase;

public class GetLatestQuoteUseCase(
    IQuoteRepository quoteRepository,
    IAssetRepository assetRepository,
    ILogger<GetLatestQuoteUseCase> logger
) : IGetLatestQuoteUseCase
{
    private readonly IQuoteRepository _quoteRepository = quoteRepository;
    private readonly IAssetRepository _assetRepository = assetRepository;
    private readonly ILogger<GetLatestQuoteUseCase> _logger = logger;

    public async Task<Output> Handle(GetLatestQuoteInput input, CancellationToken cancellationToken)
    {
        Output output = new();

        var asset = await _assetRepository.GetByTickerAsync(input.TickerSymbol, cancellationToken);

        if (asset is null)
        {
            _logger.LogWarning("Asset with ticker symbol {TickerSymbol} not found.", input.TickerSymbol);

            output.AddErrorMessage("Asset not found");
            return output;
        }

        var quote = await _quoteRepository.GetLatestQuoteByAssetIdAsync(asset.Id, cancellationToken);

        if (quote is null)
        {
            _logger.LogWarning("Quote for asset with ID {AssetId} not found.", asset.Id);

            output.AddErrorMessage("Quote not found");
            return output;
        }

        quote.Asset = asset;
        output.AddResult(quote.MapToDto());
        return output;
    }
}
