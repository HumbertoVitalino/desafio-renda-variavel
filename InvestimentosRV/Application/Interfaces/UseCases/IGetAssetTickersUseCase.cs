using Application.Commons;
using Application.UseCases.GetAssetTickersUseCase.Boundaries;

namespace Application.Interfaces.UseCases;

public interface IGetAssetTickersUseCase
{
    Task<Output> Handle(GetAssetTickersInput input, CancellationToken cancellationToken);
}
