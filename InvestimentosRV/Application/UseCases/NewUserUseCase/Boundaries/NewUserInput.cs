using Domain.Enums;

namespace Application.UseCases.NewUserUseCase.Boundaries;

public sealed record NewUserInput(
      string Name,
      string Email,
      string Password,
      string Confirmation,
      InvestorProfile Profile
);
