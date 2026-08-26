using Application.Commons;
using Application.Interfaces.Repositories;
using Application.Interfaces.UseCases;
using Application.Mappers;
using Application.UseCases.GetAllAssetsUseCase.Boundaries;

namespace Application.UseCases.GetAllAssetsUseCase;

public class GetAllAssetsUseCase(
    IAssetRepository assetRepository
) : IGetAllAssetsUseCase
{
    private readonly IAssetRepository _assetRepository = assetRepository;

    public async Task<Output> Handle(GetAllAssetsInput input, CancellationToken cancellationToken)
    {
        Output output = new();

        var assets = await _assetRepository.GetAllAssetsAsync(cancellationToken);
        output.AddResult(assets.MapToDto());
        return output;
    }
}
