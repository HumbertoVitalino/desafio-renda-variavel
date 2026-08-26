using Moq;
using AutoBogus;
using Domain.Entities;
using Application.Interfaces.Repositories;
using Application.UseCases.GetAssetTickersUseCase;
using Application.UseCases.GetAssetTickersUseCase.Boundaries;

namespace UnitTests.UseCasesTest;

public class GetAssetTickersTest
{
    private readonly GetAssetTickersUseCase _useCase;
    private readonly Mock<IAssetRepository> _assetRepositoryMock;

    public GetAssetTickersTest()
    {
        _assetRepositoryMock = new Mock<IAssetRepository>();
        _useCase = new GetAssetTickersUseCase(_assetRepositoryMock.Object);
    }

    [Fact(DisplayName = "Handle > Success > Should return the ticker symbols of all assets")]
    public async Task Handle_ShouldReturnAllTickerSymbols()
    {
        // Arrange
        var assets = new List<Asset>
        {
            new("PETR4", "Petrobras", Domain.Enums.AssetRisk.High),
            new("VALE3", "Vale", Domain.Enums.AssetRisk.Medium)
        };
        _assetRepositoryMock.Setup(r => r.GetAllAssetsAsync(It.IsAny<CancellationToken>())).ReturnsAsync(assets);

        // Act
        var result = await _useCase.Handle(new GetAssetTickersInput(), CancellationToken.None);

        // Assert
        Assert.True(result.IsValid);
        var tickers = result.GetResult<IEnumerable<string>>();
        Assert.NotNull(tickers);
        Assert.Equal(["PETR4", "VALE3"], tickers);
    }
}
