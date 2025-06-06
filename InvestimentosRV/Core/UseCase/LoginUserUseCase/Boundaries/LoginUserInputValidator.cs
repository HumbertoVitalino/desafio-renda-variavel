using FluentValidation;

namespace Core.UseCase.LoginUserUseCase.Boundaries;

public class LoginUserInputValidator : AbstractValidator<LoginUserInput>
{
	public LoginUserInputValidator()
	{
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("The email is required.")
            .EmailAddress().WithMessage("The email format is invalid.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("The password is required.")
            .MinimumLength(8).WithMessage("The password must be at least 8 characters long.")
            .Matches("[A-Z]").WithMessage("The password must contain at least one uppercase letter.")
            .Matches("[a-z]").WithMessage("The password must contain at least one lowercase letter.")
            .Matches("[0-9]").WithMessage("The password must contain at least one number.");
    }
}
