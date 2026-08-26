using AutoBogus;
using Domain.Entities;
using Application.Mappers;

namespace UnitTests.MappersTest;

public class PositionMapperTest
{
    [Fact(DisplayName = "MapPositionToDomain > Success > Should Map input data to Position")]
    public void MapPositionToDomain_GivenValidInput_ShouldMapToEntitySuccessfully()
    {
        // Arrange
        var userId = new Random().Next(1, 1000);
        var assetId = new Random().Next(1, 1000);
        var quantity = new Random().Next(1, 100);
        var executionPrice = new Random().Next(1, 100);

        // Act
        var result = PositionMapper.MapPositionToDomain(userId, assetId, quantity, executionPrice);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(userId, result.UserId);
        Assert.Equal(assetId, result.AssetId);
        Assert.Equal(quantity, result.Quantity);
        Assert.Equal(executionPrice, result.AveragePrice);
    }

    [Fact(DisplayName = "MapToDto > Success > Should Map Position to PositionDto")]
    public void MapToDto_GivenValidDomainObject_ShouldMapToDtoSuccessfully()
    {
        // Arrange
        var position = new AutoFaker<Position>()
            .Generate();

        // Act
        var result = position.MapToDto();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(position.Asset.TickerSymbol, result.TickerSymbol);
        Assert.Equal(position.Asset.Name, result.AssetName);
        Assert.Equal(position.Quantity, result.Quantity);
        Assert.Equal(position.AveragePrice, result.AveragePrice);
        Assert.Equal(position.ProfitAndLoss, result.CurrentProfitAndLoss);
    }

    [Fact(DisplayName = "MapToDto > Success > Should Map IEnumerable<Position> to IEnumerable<PositionDto>")]
    public void MapToDto_GivenValidDomainCollection_ShouldMapToDtoCollectionSuccessfully()
    {
        // Arrange
        var positions = new AutoFaker<Position>()
            .Generate(5);

        // Act
        var result = positions.MapToDto();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(5, result.Count());

        foreach (var position in positions)
        {
            var dto = result.FirstOrDefault(p => p.TickerSymbol == position.Asset.TickerSymbol);
            Assert.NotNull(dto);
            Assert.Equal(position.Asset.Name, dto.AssetName);
            Assert.Equal(position.Quantity, dto.Quantity);
            Assert.Equal(position.AveragePrice, dto.AveragePrice);
            Assert.Equal(position.ProfitAndLoss, dto.CurrentProfitAndLoss);
        }
    }
}
