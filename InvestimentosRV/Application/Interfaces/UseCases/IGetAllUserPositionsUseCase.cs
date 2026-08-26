using Application.Commons;
using Application.UseCases.GetAllUserPositionsUseCase.Boundaries;

namespace Application.Interfaces.UseCases;

public interface IGetAllUserPositionsUseCase
{
    Task<Output> Handle(GetAllUserPositionsInput input, CancellationToken cancellationToken);
}
