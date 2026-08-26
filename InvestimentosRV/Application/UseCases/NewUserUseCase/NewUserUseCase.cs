using Application.Commons;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Application.Interfaces.UseCases;
using Application.Mappers;
using Application.UseCases.NewUserUseCase.Boundaries;
using Domain.Enums;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.NewUserUseCase;

public class NewUserUseCase(
    IUserRepository userRepository,
    IPasswordService passwordService,
    IUnitOfWork unitOfWork,
    ILogger<NewUserUseCase> logger
) : INewUserUseCase
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IPasswordService _passwordService = passwordService;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly ILogger<NewUserUseCase> _logger = logger;

    public async Task<Output> Handle(NewUserInput input, CancellationToken cancellationToken)
    {
        Output output = new();

        var existingUser = await _userRepository.GetByEmailAsync(input.Email, cancellationToken);

        if (existingUser is not null)
        {
            _logger.LogWarning("Attempt to create a user with an existing email: {Email}", input.Email);

            output.AddErrorMessage("This email already exists.");
            return output;
        }

        var (passwordHash, passwordSalt) = _passwordService.CreatePasswordHash(input.Password);

        var brokerageRate = input.Profile.GetBrokerageRate();
        _logger.LogInformation("Brokerage rate for profile {Profile}: {Rate}", input.Profile, brokerageRate);

        var user = UserMapper.MapToDomain(input.Name, input.Email, passwordHash, passwordSalt, brokerageRate, input.Profile);

        await _userRepository.CreateAsync(user, cancellationToken);
        await _unitOfWork.CommitAsync(cancellationToken);

        _logger.LogInformation("New user created with email: {Email}", input.Email);

        output.AddResult(user.MapToDto());
        return output;
    }
}
