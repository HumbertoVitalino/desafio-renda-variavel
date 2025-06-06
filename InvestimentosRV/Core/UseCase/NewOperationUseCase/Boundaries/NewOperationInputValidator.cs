using FluentValidation;

namespace Core.UseCase.NewOperationUseCase.Boundaries;

public class NewOperationInputValidator : AbstractValidator<NewOperationInput>
{
    public NewOperationInputValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.TickerSymbol).NotEmpty().WithMessage("The asset's ticker is required.");
        RuleFor(x => x.Quantity).GreaterThan(0).WithMessage("The quantity must be greater than zero.");
        RuleFor(x => x.Type).IsInEnum().WithMessage("The operation type is invalid.");
    }
}
