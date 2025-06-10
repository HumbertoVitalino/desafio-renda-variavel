using AutoBogus;
using Core.Mappers;
using Core.Domain;

namespace UnitTests.MappersTest;

public class QuoteMapperTest
{
    [Fact(DisplayName = "MapToDto > Success > Should Map Quote to QuoteDto")]
    public void MapToDto_GivenValidDomainObject_ShouldMapToDtoSuccessfully()
    {
        // Assert
        var quote = new AutoFaker<Quote>().Generate();

        // Act
        var result = quote.MapToDto();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(quote.Asset.TickerSymbol, result.TickerSymbol);
        Assert.Equal(quote.Asset.Name, result.AssetName);
        Assert.Equal(quote.UnitPrice, result.UnitPrice);
        Assert.Equal(quote.DateTime, result.DateTime);
    }
}
