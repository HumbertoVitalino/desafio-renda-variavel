using AutoBogus;
using Core.Domain;
using Core.Domain.Enums;
using Core.Dtos;
using Core.Interfaces;
using Core.UseCase.NewUserUseCase;
using Core.UseCase.NewUserUseCase.Boundaries;
using Microsoft.Extensions.Logging;
using Moq;

namespace UnitTests.UseCasesTest;

public class NewUserTest
{
    private readonly NewUser _useCase;
    private readonly Mock<IUserRepository> _repositoryMock;
    private readonly Mock<IPasswordService> _passwordServiceMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<ILogger<NewUser>> _loggerMock;

    public NewUserTest()
    {
        _repositoryMock = new Mock<IUserRepository>();
        _passwordServiceMock = new Mock<IPasswordService>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _loggerMock = new Mock<ILogger<NewUser>>();
        _useCase = new NewUser(_repositoryMock.Object, _passwordServiceMock.Object, _unitOfWorkMock.Object, _loggerMock.Object);
    }

    [Fact(DisplayName = "Handle > Erro > Deve retornar erro quando e-mail já existir")]
    public async Task Handle_ShouldReturnError_WhenEmailAlreadyExists()
    {
        // Arrange
        var user = AutoFaker.Generate<User>();
        var input = new AutoFaker<NewUserInput>().Generate();
        _repositoryMock.Setup(r => r.GetByEmailAsync(input.Email, It.IsAny<CancellationToken>()))
                       .ReturnsAsync(user);

        // Act
        var result = await _useCase.Handle(input, CancellationToken.None);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains("This email already exists.", result.ErrorMessages);
        _repositoryMock.Verify(r => r.CreateAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()), Times.Never);
        _unitOfWorkMock.Verify(u => u.CommitAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Theory(DisplayName = "Handle > Sucesso > Deve criar usuário com a taxa de corretagem correta")]
    [InlineData(InvestorProfile.Conservative, 0.0050)]
    [InlineData(InvestorProfile.Moderate, 0.0025)]
    [InlineData(InvestorProfile.Bold, 0.0010)]
    public async Task Handle_ShouldCreateUser_WhenInputIsValid(InvestorProfile profile, decimal expectedRate)
    {
        // Arrange
        var input = new AutoFaker<NewUserInput>()
            .RuleFor(x => x.Profile, profile)
            .Generate();

        var passwordHash = new byte[] { 1, 2, 3 };
        var passwordSalt = new byte[] { 4, 5, 6 };

        _repositoryMock.Setup(r => r.GetByEmailAsync(input.Email, It.IsAny<CancellationToken>()))
                       .ReturnsAsync((User?)null);

        _passwordServiceMock.Setup(p => p.CreatePasswordHash(input.Password))
                            .Returns((passwordHash, passwordSalt));

        // Act
        var result = await _useCase.Handle(input, CancellationToken.None);

        // Assert
        var userDto = result.GetResult<UserDto>();
        Assert.True(result.IsValid);
        Assert.NotNull(userDto);
        Assert.Equal(input.Email, userDto.Email);
        Assert.Equal(expectedRate, userDto.BrokerageRate);

        _repositoryMock.Verify(r => r.CreateAsync(
            It.Is<User>(user =>
                user.Email == input.Email &&
                user.Name == input.Name &&
                user.Profile == profile &&
                user.BrokerageRate == (decimal)expectedRate &&
                user.PasswordHash == passwordHash
            ),
            It.IsAny<CancellationToken>()),
            Times.Once);

        _unitOfWorkMock.Verify(u => u.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}