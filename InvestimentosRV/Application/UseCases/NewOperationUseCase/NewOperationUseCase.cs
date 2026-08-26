using Application.Commons;
using Application.Interfaces.Repositories;
using Application.Interfaces.UseCases;
using Application.Mappers;
using Application.UseCases.NewOperationUseCase.Boundaries;
using Domain.Abstractions;
using Domain.Enums;
using Domain.Errors;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.NewOperationUseCase;

public class NewOperationUseCase(
    IAssetRepository assetRepository,
    IPositionRepository positionRepository,
    IOperationRepository operationRepository,
    IUserRepository userRepository,
    IQuoteRepository quoteRepository,
    IUnitOfWork unitOfWork,
    ILogger<NewOperationUseCase> logger
) : INewOperationUseCase
{
    private readonly IAssetRepository _assetRepository = assetRepository;
    private readonly IPositionRepository _positionRepository = positionRepository;
    private readonly IOperationRepository _operationRepository = operationRepository;
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IQuoteRepository _quoteRepository = quoteRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly ILogger<NewOperationUseCase> _logger = logger;

    public async Task<Output> Handle(NewOperationInput input, CancellationToken cancellationToken)
    {
        var output = new Output();

        var user = await _userRepository.GetAsync(input.UserId, cancellationToken);
        var asset = await _assetRepository.GetByTickerAsync(input.TickerSymbol, cancellationToken);
        var quote = await _quoteRepository.GetLatestQuoteByAssetIdAsync(asset!.Id, cancellationToken);
        var position = await _positionRepository.GetByUserIdAndAssetIdAsync(user!.Id, asset.Id, cancellationToken);

        var currentUnitPrice = quote!.UnitPrice;
        var totalValue = input.Quantity * currentUnitPrice;
        var calculatedBrokerageFee = totalValue * user.BrokerageRate;

        if (input.Type == OperationType.Buy && !user.IsSuitableFor(asset))
        {
            _logger.LogWarning("Trade not suitable for user profile {Profile} and asset risk {Risk}.", user.Profile, asset.Risk);
            output.AddMessage($"Trade not suitable for your profile ({user.Profile}) and the asset's risk ({asset.Risk}).");
        }

        if (input.Type == OperationType.Sell)
        {
            if (position is null)
                throw new DomainException(PositionErrors.InsufficientQuantity);

            position.ApplySell(input.Quantity);
        }
        else if (position is null)
        {
            position = PositionMapper.MapPositionToDomain(input.UserId, asset.Id, input.Quantity, currentUnitPrice);
        }
        else
        {
            position.ApplyBuy(input.Quantity, currentUnitPrice);
        }

        if (position.Id == 0)
            await _positionRepository.CreateAsync(position, cancellationToken);

        var operation = OperationMapper.MapToDomain(input.UserId, asset.Id, input.Quantity, currentUnitPrice, input.Type, calculatedBrokerageFee);

        await _operationRepository.CreateAsync(operation, cancellationToken);

        await _unitOfWork.CommitAsync(cancellationToken);

        _logger.LogInformation("Operation {OperationId} registered successfully.", operation.Id);

        operation.Asset = asset;
        output.AddResult(operation.MapToDto());
        return output;
    }
}
