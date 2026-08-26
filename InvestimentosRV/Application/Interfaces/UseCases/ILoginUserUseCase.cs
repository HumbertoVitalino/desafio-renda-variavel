using Application.Commons;
using Application.UseCases.LoginUserUseCase.Boundaries;

namespace Application.Interfaces.UseCases;

public interface ILoginUserUseCase
{
    Task<Output> Handle(LoginUserInput input, CancellationToken cancellationToken);
}
