using Core.Commons;
using MediatR;

namespace Core.UseCase.LoginUserUseCase.Boundaries;

public sealed record LoginUserInput(
    string Email,
    string Password
) : IRequest<Output>;
