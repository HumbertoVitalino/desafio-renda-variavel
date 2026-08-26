using Application.Commons;
using Application.UseCases.NewOperationUseCase.Boundaries;

namespace Application.Interfaces.UseCases;

public interface INewOperationUseCase
{
    Task<Output> Handle(NewOperationInput input, CancellationToken cancellationToken);
}
