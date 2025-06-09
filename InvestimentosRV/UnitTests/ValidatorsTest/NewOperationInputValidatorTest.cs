using Moq;
using AutoBogus;
using FluentValidation.TestHelper;
using Core.Domain;
using Core.Domain.Enums;
using Core.Interfaces;
using Core.UseCase.NewOperationUseCase.Boundaries;

namespace UnitTests.ValidatorsTest;

public class NewOperationInputValidatorTest
{
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<IAssetRepository> _assetRepositoryMock;
    private readonly Mock<IPositionRepository> _positionRepositoryMock;
    private readonly NewOperationInputValidator _validator;

    public NewOperationInputValidatorTest()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _assetRepositoryMock = new Mock<IAssetRepository>();
        _positionRepositoryMock = new Mock<IPositionRepository>();

        _validator = new NewOperationInputValidator(
            _userRepositoryMock.Object,
            _assetRepositoryMock.Object,
            _positionRepositoryMock.Object
        );
    }

    [Fact(DisplayName = "Validator > Failure > Should have error when quantity is zero")]
    public void Validator_ShouldHaveError_WhenQuantityIsZero()
    {
        // Arrange  
        var input = new AutoFaker<NewOperationInput>().RuleFor(x => x.Quantity, 0).Generate();

        // Act & Assert  
        var result = _validator.TestValidate(input);
        result.ShouldHaveValidationErrorFor(x => x.Quantity)
              .WithErrorMessage("The quantity must be greater than zero.");
    }

    [Fact(DisplayName = "Validator > Failure > Should have error when user does not exist")]
    public async Task Validator_ShouldHaveError_WhenUserDoesNotExist()
    {
        // Arrange  
        var input = new AutoFaker<NewOperationInput>().Generate();
        _userRepositoryMock.Setup(r => r.GetAsync(input.UserId, It.IsAny<CancellationToken>()))
                           .ReturnsAsync((User?)null);

        // Act  
        var result = await _validator.TestValidateAsync(input);

        // Assert  
        result.ShouldHaveValidationErrorFor(x => x.UserId)
              .WithErrorMessage($"User with ID {input.UserId} not found.");
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

    [Fact(DisplayName = "Validator > Failure > Should have error on sell when position is insufficient")]
    public async Task Validator_ShouldHaveError_OnSellWhenPositionIsInsufficient()
    {
        // Arrange  
        var user = AutoFaker.Generate<User>();
        var input = new AutoFaker<NewOperationInput>()
            .RuleFor(x => x.Type, OperationType.Sell)
            .RuleFor(x => x.Quantity, 101)
            .Generate();

        var asset = new AutoFaker<Asset>().Generate();
        var position = new AutoFaker<Position>().RuleFor(p => p.Quantity, 100).Generate();

        _userRepositoryMock.Setup(r => r.GetAsync(input.UserId, It.IsAny<CancellationToken>())).ReturnsAsync(user);
        _assetRepositoryMock.Setup(r => r.GetByTickerAsync(input.TickerSymbol, It.IsAny<CancellationToken>())).ReturnsAsync(asset);
        _positionRepositoryMock.Setup(r => r.GetByUserIdAndAssetIdAsync(input.UserId, asset.Id, It.IsAny<CancellationToken>())).ReturnsAsync(position);

        // Act  
        var result = await _validator.TestValidateAsync(input);

        // Assert  
        result.ShouldHaveValidationErrorFor(x => x)
              .WithErrorMessage("Insufficient assets to perform the sale.");
    }

    [Fact(DisplayName = "Validator > Success > Should not have errors for valid sell input")]
    public async Task Validator_ShouldNotHaveErrors_ForValidSellInput()
    {
        // Arrange  
        var user = AutoFaker.Generate<User>();
        var input = new AutoFaker<NewOperationInput>()
            .RuleFor(x => x.Type, OperationType.Sell)
            .RuleFor(x => x.Quantity, 50)
            .Generate();

        var asset = new AutoFaker<Asset>().Generate();
        var position = new AutoFaker<Position>().RuleFor(p => p.Quantity, 100).Generate();

        _userRepositoryMock.Setup(r => r.GetAsync(input.UserId, It.IsAny<CancellationToken>())).ReturnsAsync(user);
        _assetRepositoryMock.Setup(r => r.GetByTickerAsync(input.TickerSymbol, It.IsAny<CancellationToken>())).ReturnsAsync(asset);
        _positionRepositoryMock.Setup(r => r.GetByUserIdAndAssetIdAsync(input.UserId, asset.Id, It.IsAny<CancellationToken>())).ReturnsAsync(position);

        // Act  
        var result = await _validator.TestValidateAsync(input);

        // Assert  
        result.ShouldNotHaveAnyValidationErrors();
    }
}
