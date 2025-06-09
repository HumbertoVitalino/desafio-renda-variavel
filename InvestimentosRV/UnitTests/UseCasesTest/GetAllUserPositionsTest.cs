using Moq;
using AutoBogus;
using Core.Domain;
using Core.Interfaces;
using Core.UseCase.GetAllUserPositionsUseCase;
using Core.UseCase.GetAllUserPositionsUseCase.Boundaries;
using Microsoft.Extensions.Logging;
using Core.Dtos;

namespace UnitTests.UseCasesTest;

public class GetAllUserPositionsTest
{
    private readonly GetAllUserPositions _useCase;
    private readonly Mock<IPositionRepository> _positionRepositoryMock;
    private readonly Mock<ILogger<GetAllUserPositions>> _loggerMock;

    public GetAllUserPositionsTest()
    {
        _positionRepositoryMock = new Mock<IPositionRepository>();
        _loggerMock = new Mock<ILogger<GetAllUserPositions>>();
        _useCase = new GetAllUserPositions(_positionRepositoryMock.Object, _loggerMock.Object);
    }

    [Fact(DisplayName = "GetAllUserPositions > Error > Should return error when no positions are found")]
    public async Task Handle_ShouldReturnError_WhenNoPositionsFound()
    {
        // Arrange  
        var input = new AutoFaker<GetAllUserPositionsInput>().Generate();
        _positionRepositoryMock.Setup(r => r.GetAllByUserIdAsync(input.UserId, It.IsAny<CancellationToken>()))
                               .ReturnsAsync([]);

        // Act  
        var result = await _useCase.Handle(input, CancellationToken.None);

        // Assert  
        Assert.False(result.IsValid);
        Assert.Contains($"No positions found for user with ID {input.UserId}.", result.ErrorMessages);
    }

    [Fact(DisplayName = "GetAllUserPositions > Success > Should return list of position DTOs")]
    public async Task Handle_ShouldReturnPositionDtoList_WhenPositionsAreFound()
    {
        // Arrange  
        var input = new AutoFaker<GetAllUserPositionsInput>().Generate();
        var positions = new AutoFaker<Position>().Generate(3);

        _positionRepositoryMock.Setup(r => r.GetAllByUserIdAsync(input.UserId, It.IsAny<CancellationToken>()))
                               .ReturnsAsync(positions);

        // Act  
        var result = await _useCase.Handle(input, CancellationToken.None);

        // Assert  
        Assert.True(result.IsValid);
        Assert.Empty(result.ErrorMessages);

        var dtoList = result.GetResult<IEnumerable<PositionDto>>();
        Assert.NotNull(dtoList);
        Assert.Equal(3, dtoList.Count());
        Assert.Equal(positions.First().Asset.Name, dtoList.First().AssetName);
    }
}
