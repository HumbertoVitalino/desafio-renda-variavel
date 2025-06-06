using Core.Commons;
using MediatR;

namespace Core.UseCase.NewUserUseCase.Boundaries;

public sealed record NewUserInput(
      string Name,
      string Email,
      string Password,
      string Confirmation
) : IRequest<Output>;
