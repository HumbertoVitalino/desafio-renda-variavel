using Application.Commons;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Application.Interfaces.UseCases;
using Application.Mappers;
using Application.UseCases.LoginUserUseCase.Boundaries;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.LoginUserUseCase;

public class LoginUserUseCase(
    IUserRepository userRepository,
    ITokenProvider tokenProvider,
    IPasswordService passwordService,
    ILogger<LoginUserUseCase> logger
) : ILoginUserUseCase
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly ITokenProvider _tokenProvider = tokenProvider;
    private readonly IPasswordService _passwordService = passwordService;
    private readonly ILogger<LoginUserUseCase> _logger = logger;

    public async Task<Output> Handle(LoginUserInput input, CancellationToken cancellationToken)
    {
        Output output = new();

        var user = await _userRepository.GetByEmailAsync(input.Email, cancellationToken);

        if (user is null)
        {
            _logger.LogWarning("Invalid login attempt for user: {Email}", input.Email);

            output.AddErrorMessage("Invalid login attempt.");
            return output;
        }

        var isPasswordValid = _passwordService.VerifyPasswordHash(
            input.Password,
            user.PasswordHash,
            user.PasswordSalt
        );

        if (!isPasswordValid)
        {
            _logger.LogWarning("Invalid login attempt for user: {Email}", input.Email);

            output.AddErrorMessage("Invalid login attempt.");
            return output;
        }

        var token = _tokenProvider.GenerateToken(user.Id, user.Email);

        _logger.LogInformation("User {Email} logged in successfully.", input.Email);

        output.AddResult(user.MapToDto(token));
        return output;
    }
}
