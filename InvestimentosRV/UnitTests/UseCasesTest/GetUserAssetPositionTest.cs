using Xunit;
using Moq;
using AutoBogus;
using Core.UseCase.GetUserAssetPositionUseCase;
using Core.Interfaces;
using Microsoft.Extensions.Logging;
using Core.UseCase.GetUserAssetPositionUseCase.Boundaries;
using System.Threading.Tasks;
using System.Threading;
using Core.Domain;
using Core.Dtos;

namespace UnitTests.UseCasesTest;

public class GetUserAssetPositionTest
{
    private readonly GetUserAssetPosition _useCase;
    private readonly Mock<IAssetRepository> _assetRepositoryMock;
    private readonly Mock<IPositionRepository> _positionRepositoryMock;
    private readonly Mock<IQuoteRepository> _quoteRepositoryMock;
    private readonly Mock<ILogger<GetUserAssetPosition>> _loggerMock;

    public GetUserAssetPositionTest()
    {
        _assetRepositoryMock = new Mock<IAssetRepository>();
        _positionRepositoryMock = new Mock<IPositionRepository>();
        _quoteRepositoryMock = new Mock<IQuoteRepository>();
        _loggerMock = new Mock<ILogger<GetUserAssetPosition>>();

        _useCase = new GetUserAssetPosition(
            _assetRepositoryMock.Object,
            _positionRepositoryMock.Object,
            _quoteRepositoryMock.Object,
            _loggerMock.Object
        );
    }

    [Fact(DisplayName = "Handle > Error > Should return error when asset is not found")]
    public async Task Handle_ShouldReturnError_WhenAssetIsNotFound()
    {
        // Arrange  
        var input = new AutoFaker<GetUserAssetPositionInput>().Generate();

        _assetRepositoryMock.Setup(r => r.GetByTickerAsync(input.TickerSymbol, It.IsAny<CancellationToken>())).ReturnsAsync((Asset?)null);

        // Act  
        var result = await _useCase.Handle(input, CancellationToken.None);

        // Assert  
        Assert.False(result.IsValid);
        Assert.Contains($"Asset with ticker symbol '{input.TickerSymbol}' not found.", result.ErrorMessages);

        _positionRepositoryMock.Verify(p => p.GetByUserIdAndAssetIdAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact(DisplayName = "Handle > Error > Should return error when position is not found")]
    public async Task Handle_ShouldReturnError_WhenPositionIsNotFound()
    {
        // Arrange  
        var input = new AutoFaker<GetUserAssetPositionInput>().Generate();
        var asset = new AutoFaker<Asset>()
            .RuleFor(a => a.TickerSymbol, input.TickerSymbol)
            .Generate();

        _assetRepositoryMock.Setup(r => r.GetByTickerAsync(input.TickerSymbol, It.IsAny<CancellationToken>()))
                           .ReturnsAsync(asset);

        _positionRepositoryMock.Setup(p => p.GetByUserIdAndAssetIdAsync(input.UserId, asset.Id, It.IsAny<CancellationToken>()))
                               .ReturnsAsync((Position?)null);

        // Act  
        var result = await _useCase.Handle(input, CancellationToken.None);

        // Assert  
        Assert.False(result.IsValid);
        Assert.Contains($"Position for user {input.UserId} and asset '{asset.TickerSymbol}' not found.", result.ErrorMessages);
    }

    [Fact(DisplayName = "Handle > Success > Should return position DTO when found")]
    public async Task Handle_ShouldReturnPositionDto_WhenFound()
    {
        // Arrange  
        var input = new AutoFaker<GetUserAssetPositionInput>().Generate();
        var asset = new AutoFaker<Asset>().Generate();
        var position = new AutoFaker<Position>().Generate();

        position.Asset = asset;

        _assetRepositoryMock.Setup(r => r.GetByTickerAsync(input.TickerSymbol, It.IsAny<CancellationToken>()))
                           .ReturnsAsync(asset);

        _positionRepositoryMock.Setup(p => p.GetByUserIdAndAssetIdAsync(input.UserId, asset.Id, It.IsAny<CancellationToken>()))
                               .ReturnsAsync(position);

        // Act  
        var result = await _useCase.Handle(input, CancellationToken.None);

        // Assert  
        Assert.True(result.IsValid);
        Assert.Empty(result.ErrorMessages);

        var responseDto = result.GetResult<PositionDto>();
        Assert.NotNull(responseDto);
        Assert.Equal(position.Quantity, responseDto.Quantity);
        Assert.Equal(position.Asset.TickerSymbol, responseDto.TickerSymbol);
        Assert.Equal(position.Asset.Name, responseDto.AssetName);
    }
}
