using Core.Commons;
using Core.Domain.Enums;
using MediatR;

namespace Core.UseCase.NewUserUseCase.Boundaries;

public sealed record NewUserInput(
      string Name,
      string Email,
      string Password,
      string Confirmation,
      InvestorProfile Profile
) : IRequest<Output>;
