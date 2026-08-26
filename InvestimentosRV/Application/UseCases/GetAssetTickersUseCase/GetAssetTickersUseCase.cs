using Application.Commons;
using Application.Interfaces.Repositories;
using Application.Interfaces.UseCases;
using Application.UseCases.GetAssetTickersUseCase.Boundaries;

namespace Application.UseCases.GetAssetTickersUseCase;

public class GetAssetTickersUseCase(
    IAssetRepository assetRepository
) : IGetAssetTickersUseCase
{
    private readonly IAssetRepository _assetRepository = assetRepository;

    public async Task<Output> Handle(GetAssetTickersInput input, CancellationToken cancellationToken)
    {
        Output output = new();

        var assets = await _assetRepository.GetAllAssetsAsync(cancellationToken);
        var tickers = assets.Select(asset => asset.TickerSymbol);

        output.AddResult(tickers);
        return output;
    }
}
