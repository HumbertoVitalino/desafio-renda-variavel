using Moq;
using Application.Interfaces.Repositories;
using Application.UseCases.GetTotalBrokerageRevenueUseCase;
using Application.UseCases.GetTotalBrokerageRevenueUseCase.Boundaries;
using Application.DTOs;

namespace UnitTests.UseCasesTest;

public class GetTotalBrokerageRevenueTest
{
    private readonly GetTotalBrokerageRevenueUseCase _useCase;
    private readonly Mock<IOperationRepository> _operationRepositoryMock;

    public GetTotalBrokerageRevenueTest()
    {
        _operationRepositoryMock = new Mock<IOperationRepository>();
        _useCase = new GetTotalBrokerageRevenueUseCase(_operationRepositoryMock.Object);
    }

    [Fact(DisplayName = "Handle > Success > Should return the total brokerage revenue")]
    public async Task Handle_ShouldReturnTotalRevenue()
    {
        // Arrange
        _operationRepositoryMock.Setup(r => r.GetTotalBrokerageRevenueAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1234.56m);

        // Act
        var result = await _useCase.Handle(new GetTotalBrokerageRevenueInput(), CancellationToken.None);

        // Assert
        Assert.True(result.IsValid);
        var dto = result.GetResult<BrokerageRevenueDto>();
        Assert.NotNull(dto);
        Assert.Equal(1234.56m, dto.TotalRevenue);
    }
}
