using Xunit;
using Moq;
using AutoBogus;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Core.Domain;
using Core.Interfaces;
using Core.UseCase.GetTopClientsByPositionValueUseCase;
using Core.UseCase.GetTopClientsByPositionValueUseCase.Boundaries;
using Core.Dtos;

namespace UnitTests.UseCasesTest;

public class GetTopClientsByPositionValueTest
{
    private readonly GetTopClientsByPositionValue _useCase;
    private readonly Mock<IPositionRepository> _positionRepositoryMock;
    private readonly Mock<IQuoteRepository> _quoteRepositoryMock;

    public GetTopClientsByPositionValueTest()
    {
        _positionRepositoryMock = new Mock<IPositionRepository>();
        _quoteRepositoryMock = new Mock<IQuoteRepository>();
        _useCase = new GetTopClientsByPositionValue(_positionRepositoryMock.Object, _quoteRepositoryMock.Object);
    }

    [Fact(DisplayName = "GetTopClients > Success > Should calculate and sort clients by position value")]
    public async Task Handle_ShouldCalculateAndReturnTopClients_Correctly()
    {
        // Arrange  
        var userA = new AutoFaker<User>().RuleFor(u => u.Id, 1).RuleFor(u => u.Name, "User A").Generate();
        var userB = new AutoFaker<User>().RuleFor(u => u.Id, 2).RuleFor(u => u.Name, "User B").Generate();
        var userC = new AutoFaker<User>().RuleFor(u => u.Id, 3).RuleFor(u => u.Name, "User C").Generate();

        var assetX = new AutoFaker<Asset>().RuleFor(a => a.Id, 10).Generate();
        var assetY = new AutoFaker<Asset>().RuleFor(a => a.Id, 20).Generate();

        var allPositions = new List<Position>
       {
           new AutoFaker<Position>().RuleFor(p => p.User, userA).RuleFor(p => p.AssetId, assetX.Id).RuleFor(p => p.Quantity, 10).Generate(),
           new AutoFaker<Position>().RuleFor(p => p.User, userB).RuleFor(p => p.AssetId, assetY.Id).RuleFor(p => p.Quantity, 100).Generate(),
           new AutoFaker<Position>().RuleFor(p => p.User, userC).RuleFor(p => p.AssetId, assetX.Id).RuleFor(p => p.Quantity, 5).Generate(),
           new AutoFaker<Position>().RuleFor(p => p.User, userC).RuleFor(p => p.AssetId, assetY.Id).RuleFor(p => p.Quantity, 20).Generate()
       };

        var latestQuotes = new Dictionary<int, decimal>
       {
           { assetX.Id, 10.00m },
           { assetY.Id, 5.00m }
       };

        _positionRepositoryMock.Setup(r => r.GetAllWithDetailsAsync(It.IsAny<CancellationToken>())).ReturnsAsync(allPositions);
        _quoteRepositoryMock.Setup(r => r.GetLatestQuotesForAllAssetsAsync(It.IsAny<CancellationToken>())).ReturnsAsync(latestQuotes);

        // Act  
        var result = await _useCase.Handle(new GetTopClientsByPositionValueInput(), CancellationToken.None);

        // Assert  
        Assert.True(result.IsValid);
        var topClients = result.GetResult<List<TopClientsDto>>();

        Assert.NotNull(topClients);
        Assert.Equal(3, topClients.Count);

        Assert.Equal(userB.Id, topClients[0].UserId);
        Assert.Equal(500.00m, topClients[0].TotalPositionValue);

        Assert.Equal(userC.Id, topClients[1].UserId);
        Assert.Equal(150.00m, topClients[1].TotalPositionValue);

        Assert.Equal(userA.Id, topClients[2].UserId);
        Assert.Equal(100.00m, topClients[2].TotalPositionValue);
    }
}
