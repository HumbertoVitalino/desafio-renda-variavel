using Moq;
using AutoBogus;
using Domain.Entities;
using Application.Interfaces.Repositories;
using Application.UseCases.GetLatestQuoteUseCase;
using Application.UseCases.GetLatestQuoteUseCase.Boundaries;
using Microsoft.Extensions.Logging;
using Application.DTOs;

namespace UnitTests.UseCasesTest;

public class GetLatestQuoteTest
{
    private readonly GetLatestQuoteUseCase _useCase;
    private readonly Mock<IQuoteRepository> _quoteRepositoryMock;
    private readonly Mock<IAssetRepository> _assetRepositoryMock;
    private readonly Mock<ILogger<GetLatestQuoteUseCase>> _loggerMock;

    public GetLatestQuoteTest()
    {
        _quoteRepositoryMock = new Mock<IQuoteRepository>();
        _assetRepositoryMock = new Mock<IAssetRepository>();
        _loggerMock = new Mock<ILogger<GetLatestQuoteUseCase>>();
        _useCase = new GetLatestQuoteUseCase(_quoteRepositoryMock.Object, _assetRepositoryMock.Object, _loggerMock.Object);
    }

    [Fact(DisplayName = "GetLatestQuoteUseCase > Error > Should return error when asset is not found")]
    public async Task Handle_ShouldReturnError_WhenAssetNotFound()
    {
        // Arrange  
        var input = new AutoFaker<GetLatestQuoteInput>().Generate();
        _assetRepositoryMock.Setup(r => r.GetByTickerAsync(input.TickerSymbol, It.IsAny<CancellationToken>()))
                            .ReturnsAsync((Asset?)null);

        // Act  
        var result = await _useCase.Handle(input, CancellationToken.None);

        // Assert  
        Assert.False(result.IsValid);
        Assert.Contains("Asset not found", result.ErrorMessages);
        _quoteRepositoryMock.Verify(r => r.GetLatestQuoteByAssetIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact(DisplayName = "GetLatestQuoteUseCase > Error > Should return error when quote is not found")]
    public async Task Handle_ShouldReturnError_WhenQuoteNotFound()
    {
        // Arrange  
        var input = new AutoFaker<GetLatestQuoteInput>().Generate();
        var asset = new AutoFaker<Asset>().Generate();

        _assetRepositoryMock.Setup(r => r.GetByTickerAsync(input.TickerSymbol, It.IsAny<CancellationToken>()))
                            .ReturnsAsync(asset);
        _quoteRepositoryMock.Setup(r => r.GetLatestQuoteByAssetIdAsync(asset.Id, It.IsAny<CancellationToken>()))
                            .ReturnsAsync((Quote?)null);

        // Act  
        var result = await _useCase.Handle(input, CancellationToken.None);

        // Assert  
        Assert.False(result.IsValid);
        Assert.Contains("Quote not found", result.ErrorMessages);
    }

    [Fact(DisplayName = "GetLatestQuoteUseCase > Success > Should return quote DTO")]
    public async Task Handle_ShouldReturnQuoteDto_WhenFound()
    {
        // Arrange  
        var input = new AutoFaker<GetLatestQuoteInput>().Generate();
        var asset = new AutoFaker<Asset>().Generate();
        var quote = new AutoFaker<Quote>().Generate();

        _assetRepositoryMock.Setup(r => r.GetByTickerAsync(input.TickerSymbol, It.IsAny<CancellationToken>()))
                            .ReturnsAsync(asset);
        _quoteRepositoryMock.Setup(r => r.GetLatestQuoteByAssetIdAsync(asset.Id, It.IsAny<CancellationToken>()))
                            .ReturnsAsync(quote);

        // Act  
        var result = await _useCase.Handle(input, CancellationToken.None);

        // Assert  
        Assert.True(result.IsValid);
        var dto = result.GetResult<QuoteDto>();
        Assert.NotNull(dto);
        Assert.Equal(quote.UnitPrice, dto.UnitPrice);
        Assert.Equal(asset.TickerSymbol, dto.TickerSymbol);
    }
}
