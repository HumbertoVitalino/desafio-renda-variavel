using Application.Interfaces.Repositories;
using Domain.Enums;
using FluentValidation;

namespace Application.UseCases.NewOperationUseCase.Boundaries;

public class NewOperationInputValidator : AbstractValidator<NewOperationInput>
{
    private readonly IUserRepository _userRepository;
    private readonly IAssetRepository _assetRepository;

    public NewOperationInputValidator(
        IUserRepository userRepository,
        IAssetRepository assetRepository)
    {
        _userRepository = userRepository;
        _assetRepository = assetRepository;

        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.TickerSymbol).NotEmpty().WithMessage("The asset's ticker is required.");
        RuleFor(x => x.Quantity).GreaterThan(0).WithMessage("The quantity must be greater than zero.");
        RuleFor(x => x.Type).IsInEnum().WithMessage("The operation type is invalid.");

        When(x => x.UserId > 0, () => {
            RuleFor(x => x.UserId)
                .MustAsync(UserMustExist)
                .WithMessage(x => $"User with ID {x.UserId} not found.");
        });

        When(x => !string.IsNullOrEmpty(x.TickerSymbol), () => {
            RuleFor(x => x.TickerSymbol)
                .MustAsync(AssetMustExist)
                .WithMessage(x => $"Asset with ticker '{x.TickerSymbol}' not found.");
        });
    }

    private async Task<bool> UserMustExist(int userId, CancellationToken token)
    {
        return await _userRepository.GetAsync(userId, token) is not null;
    }

    private async Task<bool> AssetMustExist(string ticker, CancellationToken token)
    {
        return await _assetRepository.GetByTickerAsync(ticker, token) is not null;
    }
}
