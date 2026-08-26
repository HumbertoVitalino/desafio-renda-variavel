using Moq;
using AutoBogus;
using FluentValidation.TestHelper;
using Domain.Entities;
using Application.Interfaces.Repositories;
using Application.UseCases.NewOperationUseCase.Boundaries;

namespace UnitTests.ValidatorsTest;

public class NewOperationInputValidatorTest
{
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<IAssetRepository> _assetRepositoryMock;
    private readonly NewOperationInputValidator _validator;

    public NewOperationInputValidatorTest()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _assetRepositoryMock = new Mock<IAssetRepository>();

        _validator = new NewOperationInputValidator(
            _userRepositoryMock.Object,
            _assetRepositoryMock.Object
        );
    }

    [Fact(DisplayName = "Validator > Failure > Should have error when quantity is zero")]
    public async Task Validator_ShouldHaveError_WhenQuantityIsZero()
    {
        // Arrange
        var input = new AutoFaker<NewOperationInput>().RuleFor(x => x.Quantity, 0).Generate();

        // Act & Assert
        var result = await _validator.TestValidateAsync(input);
        result.ShouldHaveValidationErrorFor(x => x.Quantity)
              .WithErrorMessage("The quantity must be greater than zero.");
    }

    [Fact(DisplayName = "Validator > Failure > Should have error when asset does not exist")]
    public async Task Validator_ShouldHaveError_WhenAssetDoesNotExist()
    {
        // Arrange
        var user = AutoFaker.Generate<User>();
        var input = new AutoFaker<NewOperationInput>().Generate();
        _userRepositoryMock.Setup(r => r.GetAsync(input.UserId, It.IsAny<CancellationToken>())).ReturnsAsync(user);
        _assetRepositoryMock.Setup(r => r.GetByTickerAsync(input.TickerSymbol, It.IsAny<CancellationToken>())).ReturnsAsync((Asset?)null);

        // Act
        var result = await _validator.TestValidateAsync(input);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.TickerSymbol)
              .WithErrorMessage($"Asset with ticker '{input.TickerSymbol}' not found.");
    }

    [Fact(DisplayName = "Validator > Success > Should not have errors for a well-formed input")]
    public async Task Validator_ShouldNotHaveErrors_ForWellFormedInput()
    {
        // Arrange
        var user = AutoFaker.Generate<User>();
        var input = new AutoFaker<NewOperationInput>()
            .RuleFor(x => x.Quantity, 50)
            .Generate();

        var asset = new AutoFaker<Asset>().Generate();

        _userRepositoryMock.Setup(r => r.GetAsync(input.UserId, It.IsAny<CancellationToken>())).ReturnsAsync(user);
        _assetRepositoryMock.Setup(r => r.GetByTickerAsync(input.TickerSymbol, It.IsAny<CancellationToken>())).ReturnsAsync(asset);

        // Act
        var result = await _validator.TestValidateAsync(input);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }
}
