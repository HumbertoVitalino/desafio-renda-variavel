using Moq;
using AutoBogus;
using Core.Domain;
using Core.Domain.Enums;
using Core.Interfaces;
using Core.UseCase.NewOperationUseCase;
using Core.UseCase.NewOperationUseCase.Boundaries;
using Microsoft.Extensions.Logging;
using Core.Dtos;

namespace UnitTests.UseCasesTest;

public class NewOperationTest
{
    private readonly NewOperation _useCase;
    private readonly Mock<IAssetRepository> _assetRepositoryMock;
    private readonly Mock<IPositionRepository> _positionRepositoryMock;
    private readonly Mock<IOperationRepository> _operationRepositoryMock;
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<IQuoteRepository> _quoteRepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<ILogger<NewOperation>> _loggerMock;

    public NewOperationTest()
    {
        _assetRepositoryMock = new Mock<IAssetRepository>();
        _positionRepositoryMock = new Mock<IPositionRepository>();
        _operationRepositoryMock = new Mock<IOperationRepository>();
        _userRepositoryMock = new Mock<IUserRepository>();
        _quoteRepositoryMock = new Mock<IQuoteRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _loggerMock = new Mock<ILogger<NewOperation>>();

        _useCase = new NewOperation(
            _assetRepositoryMock.Object,
            _positionRepositoryMock.Object,
            _operationRepositoryMock.Object,
            _userRepositoryMock.Object,
            _quoteRepositoryMock.Object,
            _unitOfWorkMock.Object,
            _loggerMock.Object
        );
    }

    [Fact(DisplayName = "Handle > Success > Should create new position on first buy")]
    public async Task Handle_ShouldCreateNewPosition_OnFirstBuy()
    {
        // Arrange  
        var input = new AutoFaker<NewOperationInput>().RuleFor(x => x.Type, OperationType.Buy).Generate();
        var user = new AutoFaker<User>().Generate();
        var asset = new AutoFaker<Asset>().Generate();
        var quote = new AutoFaker<Quote>().Generate();

        _userRepositoryMock.Setup(r => r.GetAsync(input.UserId, It.IsAny<CancellationToken>())).ReturnsAsync(user);
        _assetRepositoryMock.Setup(r => r.GetByTickerAsync(input.TickerSymbol, It.IsAny<CancellationToken>())).ReturnsAsync(asset);
        _quoteRepositoryMock.Setup(r => r.GetLatestQuoteByAssetIdAsync(asset.Id, It.IsAny<CancellationToken>())).ReturnsAsync(quote);
        _positionRepositoryMock.Setup(r => r.GetByUserIdAndAssetIdAsync(user.Id, asset.Id, It.IsAny<CancellationToken>())).ReturnsAsync((Position?)null);

        // Act  
        var result = await _useCase.Handle(input, CancellationToken.None);

        // Assert  
        Assert.True(result.IsValid);
        Assert.NotNull(result.GetResult<OperationDto>());
        _positionRepositoryMock.Verify(r => r.CreateAsync(It.Is<Position>(p => p.Quantity == input.Quantity), It.IsAny<CancellationToken>()), Times.Once);
        _operationRepositoryMock.Verify(r => r.CreateAsync(It.IsAny<Operation>(), It.IsAny<CancellationToken>()), Times.Once);
        _unitOfWorkMock.Verify(u => u.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact(DisplayName = "Handle > Success > Should update position and average price on subsequent buy")]
    public async Task Handle_ShouldUpdatePositionAndAveragePrice_OnSubsequentBuy()
    {
        // Arrange  
        var input = new AutoFaker<NewOperationInput>().RuleFor(x => x.Type, OperationType.Buy).RuleFor(x => x.Quantity, 50).Generate();
        var user = new AutoFaker<User>().Generate();
        var asset = new AutoFaker<Asset>().Generate();
        var quote = new AutoFaker<Quote>().RuleFor(x => x.UnitPrice, 12.00m).Generate();
        var existingPosition = new AutoFaker<Position>().RuleFor(x => x.Quantity, 100).RuleFor(x => x.AveragePrice, 10.00m).Generate();

        _userRepositoryMock.Setup(r => r.GetAsync(input.UserId, It.IsAny<CancellationToken>())).ReturnsAsync(user);
        _assetRepositoryMock.Setup(r => r.GetByTickerAsync(input.TickerSymbol, It.IsAny<CancellationToken>())).ReturnsAsync(asset);
        _quoteRepositoryMock.Setup(r => r.GetLatestQuoteByAssetIdAsync(asset.Id, It.IsAny<CancellationToken>())).ReturnsAsync(quote);
        _positionRepositoryMock.Setup(r => r.GetByUserIdAndAssetIdAsync(user.Id, asset.Id, It.IsAny<CancellationToken>())).ReturnsAsync(existingPosition);

        // Act  
        var result = await _useCase.Handle(input, CancellationToken.None);

        // Assert  
        Assert.True(result.IsValid);
        _positionRepositoryMock.Verify(r => r.CreateAsync(It.IsAny<Position>(), It.IsAny<CancellationToken>()), Times.Never);
        _unitOfWorkMock.Verify(u => u.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
        Assert.Equal(150, existingPosition.Quantity);
        Assert.Equal(10.67m, Math.Round(existingPosition.AveragePrice, 2));
    }

    [Fact(DisplayName = "Handle > Success > Should update quantity on sell")]
    public async Task Handle_ShouldUpdateQuantity_OnSell()
    {
        // Arrange  
        var input = new AutoFaker<NewOperationInput>().RuleFor(x => x.Type, OperationType.Sell).RuleFor(x => x.Quantity, 40).Generate();
        var user = new AutoFaker<User>().Generate();
        var asset = new AutoFaker<Asset>().Generate();
        var quote = new AutoFaker<Quote>().Generate();
        var existingPosition = new AutoFaker<Position>().RuleFor(x => x.Quantity, 100).Generate();

        _userRepositoryMock.Setup(r => r.GetAsync(input.UserId, It.IsAny<CancellationToken>())).ReturnsAsync(user);
        _assetRepositoryMock.Setup(r => r.GetByTickerAsync(input.TickerSymbol, It.IsAny<CancellationToken>())).ReturnsAsync(asset);
        _quoteRepositoryMock.Setup(r => r.GetLatestQuoteByAssetIdAsync(asset.Id, It.IsAny<CancellationToken>())).ReturnsAsync(quote);
        _positionRepositoryMock.Setup(r => r.GetByUserIdAndAssetIdAsync(user.Id, asset.Id, It.IsAny<CancellationToken>())).ReturnsAsync(existingPosition);

        // Act  
        var result = await _useCase.Handle(input, CancellationToken.None);

        // Assert  
        Assert.True(result.IsValid);
        Assert.Equal(60, existingPosition.Quantity);
        _unitOfWorkMock.Verify(u => u.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact(DisplayName = "Handle > Warning > Should add warning message for unsuitable trade")]
    public async Task Handle_ShouldAddWarningMessage_ForUnsuitableTrade()
    {
        // Arrange  
        var input = new AutoFaker<NewOperationInput>().RuleFor(x => x.Type, OperationType.Buy).Generate();
        var user = new AutoFaker<User>().RuleFor(u => u.Profile, InvestorProfile.Conservative).Generate();
        var asset = new AutoFaker<Asset>().RuleFor(a => a.Risk, AssetRisk.High).Generate();
        var quote = new AutoFaker<Quote>().Generate();

        _userRepositoryMock.Setup(r => r.GetAsync(input.UserId, It.IsAny<CancellationToken>())).ReturnsAsync(user);
        _assetRepositoryMock.Setup(r => r.GetByTickerAsync(input.TickerSymbol, It.IsAny<CancellationToken>())).ReturnsAsync(asset);
        _quoteRepositoryMock.Setup(r => r.GetLatestQuoteByAssetIdAsync(asset.Id, It.IsAny<CancellationToken>())).ReturnsAsync(quote);
        _positionRepositoryMock.Setup(r => r.GetByUserIdAndAssetIdAsync(user.Id, asset.Id, It.IsAny<CancellationToken>())).ReturnsAsync((Position?)null);

        // Act  
        var result = await _useCase.Handle(input, CancellationToken.None);

        // Assert  
        Assert.True(result.IsValid);
        Assert.Contains($"Trade not suitable for your profile ({user.Profile}) and the asset's risk ({asset.Risk}).", result.Messages);
        _unitOfWorkMock.Verify(u => u.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}
