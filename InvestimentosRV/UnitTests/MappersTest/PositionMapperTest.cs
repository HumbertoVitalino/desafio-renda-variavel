using AutoBogus;
using Core.Domain;
using Core.Mappers;
using Core.UseCase.NewOperationUseCase.Boundaries;

namespace UnitTests.MappersTest;

public class PositionMapperTest
{
    [Fact(DisplayName = "MapToDomain > Success > Should Map NewOperationInput to Position")]
    public void MapToDomain_GivenValidInput_ShouldMapToEntitySuccessfully()
    {
        // Arrange  
        var assetId = new Random().Next(1, 1000);
        var executionPrice = new Random().Next(1, 100);

        var input = new AutoFaker<NewOperationInput>()
            .RuleFor(x => x.UserId, f => f.Random.Int(1, 1000))
            .RuleFor(x => x.Quantity, f => f.Random.Int(1, 100))
            .Generate();

        // Act  
        var result = input.MapPositionToDomain(assetId, executionPrice);

        // Assert  
        Assert.NotNull(result);
        Assert.Equal(input.UserId, result.UserId);
        Assert.Equal(assetId, result.AssetId);
        Assert.Equal(input.Quantity, result.Quantity);
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
