using Domain.Abstractions;
using Domain.Entities;
using Domain.Errors;

namespace UnitTests.DomainTest;

public class PositionTest
{
    [Fact(DisplayName = "ApplyBuy > Success > Should update quantity and weighted average price")]
    public void ApplyBuy_GivenAdditionalPurchase_ShouldUpdateQuantityAndAveragePrice()
    {
        // Arrange
        var position = new Position(userId: 1, assetId: 1, quantity: 100, averagePrice: 10.00m);

        // Act
        position.ApplyBuy(50, 12.00m);

        // Assert
        Assert.Equal(150, position.Quantity);
        Assert.Equal(10.67m, Math.Round(position.AveragePrice, 2));
    }

    [Fact(DisplayName = "ApplySell > Success > Should decrease quantity and keep average price")]
    public void ApplySell_GivenPartialSale_ShouldDecreaseQuantity()
    {
        // Arrange
        var position = new Position(userId: 1, assetId: 1, quantity: 100, averagePrice: 10.00m);

        // Act
        position.ApplySell(40);

        // Assert
        Assert.Equal(60, position.Quantity);
        Assert.Equal(10.00m, position.AveragePrice);
    }

    [Fact(DisplayName = "ApplySell > Success > Should zero out average price and P&L when position is fully closed")]
    public void ApplySell_GivenFullSale_ShouldZeroOutPosition()
    {
        // Arrange
        var position = new Position(userId: 1, assetId: 1, quantity: 100, averagePrice: 10.00m);

        // Act
        position.ApplySell(100);

        // Assert
        Assert.Equal(0, position.Quantity);
        Assert.Equal(0, position.AveragePrice);
        Assert.Equal(0, position.ProfitAndLoss);
    }

    [Fact(DisplayName = "ApplySell > Failure > Should throw DomainException when selling more than held quantity")]
    public void ApplySell_GivenQuantityGreaterThanHeld_ShouldThrowDomainException()
    {
        // Arrange
        var position = new Position(userId: 1, assetId: 1, quantity: 100, averagePrice: 10.00m);

        // Act
        var exception = Assert.Throws<DomainException>(() => position.ApplySell(101));

        // Assert
        Assert.Equal(PositionErrors.InsufficientQuantity, exception.Message);
    }

    [Fact(DisplayName = "UpdateProfitAndLossWithNewQuote > Success > Should recalculate P&L from the new quote")]
    public void UpdateProfitAndLossWithNewQuote_GivenNewPrice_ShouldRecalculateProfitAndLoss()
    {
        // Arrange
        var position = new Position(userId: 1, assetId: 1, quantity: 10, averagePrice: 10.00m);

        // Act
        position.UpdateProfitAndLossWithNewQuote(12.00m);

        // Assert
        Assert.Equal(20.00m, position.ProfitAndLoss);
    }
}
