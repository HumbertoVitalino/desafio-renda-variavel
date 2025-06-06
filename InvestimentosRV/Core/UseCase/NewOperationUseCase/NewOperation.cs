using Core.Commons;
using Core.Domain;
using Core.Domain.Enums;
using Core.Interfaces;
using Core.Mappers;
using Core.UseCase.NewOperationUseCase.Boundaries;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Core.UseCase.NewOperationUseCase;

public class NewOperation(
    IAssetRepository assetRepository,
    IPositionRepository positionRepository,
    IOperationRepository operationRepository,
    IUserRepository userRepository,
    IUnitOfWork unitOfWork,
    ILogger<NewOperation> logger)
    : IRequestHandler<NewOperationInput, Output>
{
    private readonly IAssetRepository _assetRepository = assetRepository;
    private readonly IPositionRepository _positionRepository = positionRepository;
    private readonly IOperationRepository _operationRepository = operationRepository;
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly ILogger<NewOperation> _logger = logger;

    public async Task<Output> Handle(NewOperationInput input, CancellationToken cancellationToken)
    {
        var output = new Output();

        var user = await _userRepository.GetAsync(input.UserId, cancellationToken);
        if (user is null)
        {
            output.AddErrorMessage($"User with ID {input.UserId} not found.");
            return output;
        }

        var asset = await _assetRepository.GetByTickerAsync(input.TickerSymbol, cancellationToken);
        if (asset is null)
        {
            output.AddErrorMessage($"Asset with ticker '{input.TickerSymbol}' not found.");
            return output;
        }

        var position = await _positionRepository.GetByUserIdAndAssetIdAsync(user.Id, asset.Id, cancellationToken);

        var suitabilityWarning = CheckSuitability(input, user, asset);
        if (suitabilityWarning is not null)
            output.AddMessage(suitabilityWarning);

        var sellValidationError = CheckSellValidity(input, position);
        if (sellValidationError is not null)
        {
            output.AddErrorMessage(sellValidationError);
            return output;
        }

        position = UpdatePositionLogic(input, asset.Id, position);

        if (position.Id == 0)
            await _positionRepository.CreateAsync(position, cancellationToken);

        var operation = input.MapToDomain(asset.Id);

        await _operationRepository.CreateAsync(operation, cancellationToken);

        await _unitOfWork.CommitAsync(cancellationToken);

        _logger.LogInformation("Operation {OperationId} registered successfully.", operation.Id);

        operation.Asset = asset;
        output.AddResult(operation.MapToDto());
        return output;
    }

    private static Position UpdatePositionLogic(NewOperationInput input, int assetId, Position? position)
    {
        if (input.Type == OperationType.Sell)
        {
            var newQuantityAfterSell = position!.Quantity - input.Quantity;
            position.UpdatePositionAfterOperation(newQuantityAfterSell, position.AveragePrice);
            return position;
        }

        if (position is null)
        {
            return input.MapPositionToDomain(assetId);
        }

        var totalValueOld = position.Quantity * position.AveragePrice;
        var totalValueNew = input.Quantity * input.UnitPrice;
        var totalQuantity = position.Quantity + input.Quantity;
        var newAveragePrice = (totalValueOld + totalValueNew) / totalQuantity;

        position.UpdatePositionAfterOperation(totalQuantity, newAveragePrice);
        return position;
    }

    private string? CheckSuitability(NewOperationInput input, User user, Asset asset)
    {
        if (input.Type == OperationType.Buy && !IsTradeSuitable(user.Profile, asset.Risk))
        {
            _logger.LogWarning("Trade not suitable for user profile {Profile} and asset risk {Risk}.", user.Profile, asset.Risk);
            return $"Trade not suitable for your profile ({user.Profile}) and the asset's risk ({asset.Risk}).";
        }
        return null;
    }

    private string? CheckSellValidity(NewOperationInput input, Position? position)
    {
        if (input.Type == OperationType.Sell && (position is null || position.Quantity < input.Quantity))
        {
            _logger.LogWarning("Insufficient assets to perform the sale for user {UserId} and asset.", input.UserId);
            return "Insufficient assets to perform the sale.";
        }
        return null;
    }

    private static bool IsTradeSuitable(InvestorProfile userProfile, AssetRisk assetRisk)
    {
        if (userProfile == InvestorProfile.Conservative && assetRisk > AssetRisk.Low) return false;
        if (userProfile == InvestorProfile.Moderate && assetRisk > AssetRisk.Medium) return false;
        return true;
    }
}