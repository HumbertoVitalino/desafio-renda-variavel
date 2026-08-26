using Application.Commons;
using Application.UseCases.GetAllAssetsUseCase.Boundaries;

namespace Application.Interfaces.UseCases;

public interface IGetAllAssetsUseCase
{
    Task<Output> Handle(GetAllAssetsInput input, CancellationToken cancellationToken);
}
