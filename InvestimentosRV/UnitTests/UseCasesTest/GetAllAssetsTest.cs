using Moq;
using AutoBogus;
using Domain.Entities;
using Application.Interfaces.Repositories;
using Application.UseCases.GetAllAssetsUseCase;
using Application.UseCases.GetAllAssetsUseCase.Boundaries;
using Application.DTOs;

namespace UnitTests.UseCasesTest;

public class GetAllAssetsTest
{
    private readonly GetAllAssetsUseCase _useCase;
    private readonly Mock<IAssetRepository> _assetRepositoryMock;

    public GetAllAssetsTest()
    {
        _assetRepositoryMock = new Mock<IAssetRepository>();
        _useCase = new GetAllAssetsUseCase(_assetRepositoryMock.Object);
    }

    [Fact(DisplayName = "Handle > Success > Should return all assets mapped to DTOs")]
    public async Task Handle_ShouldReturnAllAssetsAsDtos()
    {
        // Arrange
        var assets = new AutoFaker<Asset>().Generate(3);
        _assetRepositoryMock.Setup(r => r.GetAllAssetsAsync(It.IsAny<CancellationToken>())).ReturnsAsync(assets);

        // Act
        var result = await _useCase.Handle(new GetAllAssetsInput(), CancellationToken.None);

        // Assert
        Assert.True(result.IsValid);
        var dtoList = result.GetResult<IEnumerable<AssetDto>>();
        Assert.NotNull(dtoList);
        Assert.Equal(3, dtoList.Count());
    }
}
