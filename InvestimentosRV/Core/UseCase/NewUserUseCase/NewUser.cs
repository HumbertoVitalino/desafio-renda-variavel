using Core.Commons;
using Core.Domain.Enums;
using Core.Interfaces;
using Core.Mappers;
using Core.UseCase.NewUserUseCase.Boundaries;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Core.UseCase.NewUserUseCase;

public class NewUser(
    IUserRepository userRepository,
    IPasswordService passwordService,
    IUnitOfWork unitOfWork,
    ILogger<NewUser> logger
) : IRequestHandler<NewUserInput, Output>
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IPasswordService _passwordService = passwordService;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly ILogger<NewUser> _logger = logger;

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

        var brokerageRate = GetBrokerageRate(input.Profile);
        _logger.LogInformation("Brokerage rate for profile {Profile}: {Rate}", input.Profile, brokerageRate);

        var user = input.MapToDomain(passwordHash, passwordSalt, brokerageRate);

        await _userRepository.CreateAsync(user, cancellationToken);
        await _unitOfWork.CommitAsync(cancellationToken);

        _logger.LogInformation("New user created with email: {Email}", input.Email);

        output.AddResult(user.MapToDto());
        return output;
    }

    private static decimal GetBrokerageRate(InvestorProfile profile)
    {
        return profile switch
        {
            InvestorProfile.Bold => 0.0010m,
            InvestorProfile.Moderate => 0.0025m,
            InvestorProfile.Conservative => 0.0050m,
            _ => 0.0050m
        };
    }
}
