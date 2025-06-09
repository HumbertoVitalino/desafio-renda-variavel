using Core.Commons;
using Core.Interfaces;
using Core.Mappers;
using Core.UseCase.GetLatestQuoteUseCase.Boundaries;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Core.UseCase.GetLatestQuoteUseCase;

public class GetLatestQuote(
    IQuoteRepository quoteRepository,
    IAssetRepository assetRepository,
    ILogger<GetLatestQuote> logger
) : IRequestHandler<GetLatestQuoteInput, Output>
{
    private readonly IQuoteRepository _quoteRepository = quoteRepository;
    private readonly IAssetRepository _assetRepository = assetRepository;
    private readonly ILogger<GetLatestQuote> _logger = logger;

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
