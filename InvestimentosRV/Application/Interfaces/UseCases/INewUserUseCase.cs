using Application.Commons;
using Application.UseCases.NewUserUseCase.Boundaries;

namespace Application.Interfaces.UseCases;

public interface INewUserUseCase
{
    Task<Output> Handle(NewUserInput input, CancellationToken cancellationToken);
}
