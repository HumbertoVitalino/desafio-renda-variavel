using Core.Domain.Enums;
using Core.Interfaces;
using FluentValidation;

namespace Core.UseCase.NewOperationUseCase.Boundaries;

public class NewOperationInputValidator : AbstractValidator<NewOperationInput>
{
    private readonly IUserRepository _userRepository;
    private readonly IAssetRepository _assetRepository;
    private readonly IPositionRepository _positionRepository;

    public NewOperationInputValidator(
        IUserRepository userRepository,
        IAssetRepository assetRepository,
        IPositionRepository positionRepository)
    {
        _userRepository = userRepository;
        _assetRepository = assetRepository;
        _positionRepository = positionRepository;

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

        When(x => x.Type == OperationType.Sell, () => {
            RuleFor(x => x)
                .MustAsync(HaveEnoughAssetsForSale)
                .WithMessage("Insufficient assets to perform the sale.");
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

    private async Task<bool> HaveEnoughAssetsForSale(NewOperationInput input, CancellationToken token)
    {
        var asset = await _assetRepository.GetByTickerAsync(input.TickerSymbol, token);
        if (asset is null) return true;

        var position = await _positionRepository.GetByUserIdAndAssetIdAsync(input.UserId, asset.Id, token);

        return position is not null && position.Quantity >= input.Quantity;
    }
}