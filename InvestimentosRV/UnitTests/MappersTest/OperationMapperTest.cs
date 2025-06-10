using AutoBogus;
using Core.Mappers;
using Core.Domain.Enums;
using Core.UseCase.NewOperationUseCase.Boundaries;
using Core.Domain;
using Api.Requests;
using Api.Mappers;

namespace UnitTests.MappersTest;

public class OperationMapperTest
{
    [Fact(DisplayName = "MapToDomain > Success > Should Map NewOperationInput to Operation")]
    public void MapToDomain_GivenValidInput_ShouldMapToEntitySuccessfully()
    {
        // Arrange  
        var assetId = new Random().Next(1, 1000);
        var executionPrice = new Random().Next(1, 100);
        var brokerageFee = new Random().Next(0, 10);

        var input = new AutoFaker<NewOperationInput>()
            .RuleFor(x => x.UserId, f => f.Random.Int(1, 1000))
            .RuleFor(x => x.Quantity, f => f.Random.Int(1, 100))
            .RuleFor(x => x.Type, f => f.PickRandom<OperationType>())
            .Generate();

        // Act  
        var result = input.MapToDomain(assetId, executionPrice, brokerageFee);

        // Assert  
        Assert.NotNull(result);
        Assert.Equal(input.UserId, result.UserId);
        Assert.Equal(assetId, result.AssetId);
        Assert.Equal(input.Quantity, result.Quantity);
        Assert.Equal(executionPrice, result.UnitPrice);
        Assert.Equal(input.Type, result.Type);
        Assert.Equal(brokerageFee, result.BrokerageFee);
        Assert.True((DateTime.UtcNow - result.DateTime).TotalSeconds < 5);
    }

    [Fact(DisplayName = "MapToDto > Success > Should Map Operation to OperationDto")]
    public void MapToDto_GivenValidDomainObject_ShouldMapToDtoSuccessfully()
    {
        // Arrange  
        var operation = new AutoFaker<Operation>()
            .Generate();

        // Act  
        var result = operation.MapToDto();

        // Assert   
        Assert.NotNull(result);
        Assert.Equal(operation.Id, result.Id);
        Assert.Equal(operation.Asset.TickerSymbol, result.TickerSymbol);
        Assert.Equal(operation.Type, result.Type);
        Assert.Equal(operation.Quantity, result.Quantity);
        Assert.Equal(operation.UnitPrice, result.UnitPrice);
        Assert.Equal(operation.BrokerageFee, result.BrokerageFee);
        Assert.Equal(operation.DateTime, result.DateTime);
    }

    [Fact(DisplayName = "MapToInput > Success > Should Map NewOperationInput to NewOperationRequest")]
    public void MapToInput_GivenValidRequest_ShouldMapToInputSuccessfully()
    {
        // Arrange
        var request = new AutoFaker<NewOperationRequest>().Generate();

        // Act
        var result = request.MapToInput(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.UserId);
        Assert.Equal(request.TickerSymbol, result.TickerSymbol);
        Assert.Equal(request.Quantity, result.Quantity);
        Assert.Equal(request.Type, result.Type);
    }
}

