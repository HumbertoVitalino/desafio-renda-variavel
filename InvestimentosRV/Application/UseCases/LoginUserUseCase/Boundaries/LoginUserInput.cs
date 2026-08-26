namespace Application.UseCases.LoginUserUseCase.Boundaries;

public sealed record LoginUserInput(
    string Email,
    string Password
);
