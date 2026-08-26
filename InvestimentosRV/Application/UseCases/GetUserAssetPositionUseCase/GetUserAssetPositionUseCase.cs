using Application.Commons;
using Application.Interfaces.Repositories;
using Application.Interfaces.UseCases;
using Application.Mappers;
using Application.UseCases.GetUserAssetPositionUseCase.Boundaries;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.GetUserAssetPositionUseCase;

public class GetUserAssetPositionUseCase(
    IAssetRepository assetRepository,
    IPositionRepository positionRepository,
    ILogger<GetUserAssetPositionUseCase> logger
) : IGetUserAssetPositionUseCase
{
    private readonly IAssetRepository _assetRepository = assetRepository;
    private readonly IPositionRepository _positionRepository = positionRepository;
    private readonly ILogger<GetUserAssetPositionUseCase> _logger = logger;

    public async Task<Output> Handle(GetUserAssetPositionInput input, CancellationToken cancellationToken)
    {
        Output output = new();

        var asset = await _assetRepository.GetByTickerAsync(input.TickerSymbol, cancellationToken);
        if (asset is null)
        {
            _logger.LogWarning("Asset with ticker symbol '{TickerSymbol}' not found for user {UserId}.", input.TickerSymbol, input.UserId);

            output.AddErrorMessage($"Asset with ticker symbol '{input.TickerSymbol}' not found.");
            return output;
        }

        var position = await _positionRepository.GetByUserIdAndAssetIdAsync(input.UserId, asset.Id, cancellationToken);
        if (position is null)
        {
            _logger.LogWarning("Position for user {UserId} and asset {AssetId} not found.", input.UserId, asset.Id);

            output.AddErrorMessage($"Position for user {input.UserId} and asset '{asset.TickerSymbol}' not found.");
            return output;
        }

        output.AddResult(position.MapToDto());
        return output;
    }
}
