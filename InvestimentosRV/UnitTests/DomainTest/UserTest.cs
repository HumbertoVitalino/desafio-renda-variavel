using Domain.Entities;
using Domain.Enums;

namespace UnitTests.DomainTest;

public class UserTest
{
    private static User CreateUser(InvestorProfile profile)
    {
        return new User("Test User", "test@example.com", [], [], 0.0010m, profile);
    }

    private static Asset CreateAsset(AssetRisk risk)
    {
        return new Asset("TEST4", "Test Asset", risk);
    }

    [Theory(DisplayName = "IsSuitableFor > Success > Conservative investors are only suitable for low-risk assets")]
    [InlineData(AssetRisk.Low, true)]
    [InlineData(AssetRisk.Medium, false)]
    [InlineData(AssetRisk.High, false)]
    public void IsSuitableFor_GivenConservativeProfile_ShouldOnlyAllowLowRisk(AssetRisk risk, bool expectedSuitable)
    {
        // Arrange
        var user = CreateUser(InvestorProfile.Conservative);
        var asset = CreateAsset(risk);

        // Act
        var result = user.IsSuitableFor(asset);

        // Assert
        Assert.Equal(expectedSuitable, result);
    }

    [Theory(DisplayName = "IsSuitableFor > Success > Moderate investors are suitable up to medium-risk assets")]
    [InlineData(AssetRisk.Low, true)]
    [InlineData(AssetRisk.Medium, true)]
    [InlineData(AssetRisk.High, false)]
    public void IsSuitableFor_GivenModerateProfile_ShouldAllowUpToMediumRisk(AssetRisk risk, bool expectedSuitable)
    {
        // Arrange
        var user = CreateUser(InvestorProfile.Moderate);
        var asset = CreateAsset(risk);

        // Act
        var result = user.IsSuitableFor(asset);

        // Assert
        Assert.Equal(expectedSuitable, result);
    }

    [Theory(DisplayName = "IsSuitableFor > Success > Bold investors are suitable for any risk level")]
    [InlineData(AssetRisk.Low)]
    [InlineData(AssetRisk.Medium)]
    [InlineData(AssetRisk.High)]
    public void IsSuitableFor_GivenBoldProfile_ShouldAllowAnyRisk(AssetRisk risk)
    {
        // Arrange
        var user = CreateUser(InvestorProfile.Bold);
        var asset = CreateAsset(risk);

        // Act
        var result = user.IsSuitableFor(asset);

        // Assert
        Assert.True(result);
    }
}
