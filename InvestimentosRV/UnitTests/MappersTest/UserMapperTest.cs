using Api.Requests;
using Api.Mappers;
using AutoBogus;
using Core.Domain;
using Core.Mappers;
using Core.UseCase.NewUserUseCase.Boundaries;
using System.Security.Cryptography;

namespace UnitTests.MappersTest;

public class UserMapperTest
{
    [Fact(DisplayName = "MapToDomain > Success > Should Map NewUserInput to User")]
    public void MapToDomain_GivenValidInput_ShouldMapToEntitySuccessfully()
    {
        // Arrange
        var input = new AutoFaker<NewUserInput>().Generate();
        var (passwordHash, passwordSalt) = CreatePasswordHash(input.Password);
        var brokerageRate = new Random().Next(1, 100);

        // Act
        var result = input.MapToDomain(passwordHash, passwordSalt, brokerageRate);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(input.Name, result.Name);
        Assert.Equal(input.Email, result.Email);
        Assert.Equal(passwordHash, result.PasswordHash);
        Assert.Equal(passwordSalt, result.PasswordSalt);
        Assert.Equal(brokerageRate, result.BrokerageRate);
        Assert.Equal(input.Profile, result.Profile);
    }

    [Fact(DisplayName = "MapToDto > Success > Should Map User to UserDto")]
    public void MapToDto_GivenValidDomainObject_ShouldMapToDtoSuccessfully()
    {
        // Arrange
        var user = new AutoFaker<User>().Generate();

        // Act
        var result = user.MapToDto();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(user.Name, result.Name);
        Assert.Equal(user.Email, result.Email);
        Assert.Equal(user.BrokerageRate, result.BrokerageRate);
    }

    [Fact(DisplayName = "MapToDto > Success > Should Map User to UserLoginDto")]
    public void MapToDto_GivenValidDomainObject_ShouldMapToLoginDtoSuccessfully()
    {
        // Arrange
        var user = new AutoFaker<User>().Generate();
        var token = new AutoFaker<string>().Generate();

        // Act
        var result = user.MapToDto(token);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(user.Id, result.Id);
        Assert.Equal(user.Name, result.Name);
        Assert.Equal(user.Email, result.Email);
        Assert.Equal(token, result.Token);
    }

    [Fact(DisplayName = "MapToInput > Success > Should Map NewUserRequest to NewUserInput")]
    public void MapToInput_GivenValidRequest_ShouldMapToInputSuccessfully()
    {
        // Arrange
        var password = new AutoFaker<string>();
        var request = new AutoFaker<NewUserRequest>()
            .RuleFor(x => x.Password, password)
            .RuleFor(x => x.Confirmation, password)
            .Generate();

        // Act
        var result = request.MapToInput();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(request.Name, result.Name);
        Assert.Equal(request.Email, result.Email);
        Assert.Equal(request.Password, result.Password);
        Assert.Equal(request.Confirmation, result.Confirmation);
        Assert.Equal(request.Profile, result.Profile);
    }

    [Fact(DisplayName = "MapToInput > Success > Should Map LoginUserRequest to LoginUserInput")]
    public void MapToInput_GivenValidRequest_ShouldMapToLoginInputSuccessfully()
    {
        // Arrange
        var request = new AutoFaker<LoginUserRequest>().Generate();

        // Act
        var result = request.MapToInput();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(request.Email, result.Email);
        Assert.Equal(request.Password, result.Password);
    }

    private static (byte[] passwordHash, byte[] passwordSalt) CreatePasswordHash(string password)
    {
        var salt = RandomNumberGenerator.GetBytes(16);

        var hash = Rfc2898DeriveBytes.Pbkdf2(
            password,
            salt,
            1000,
            HashAlgorithmName.SHA256,
            32
        );

        return (hash, salt);
    }
}
