using Application.Commons;
using Application.UseCases.GetUserAssetPositionUseCase.Boundaries;

namespace Application.Interfaces.UseCases;

public interface IGetUserAssetPositionUseCase
{
    Task<Output> Handle(GetUserAssetPositionInput input, CancellationToken cancellationToken);
}
