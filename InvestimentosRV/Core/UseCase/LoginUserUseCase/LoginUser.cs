using Core.Boundaries.Jwt;
using Core.Commons;
using Core.Interfaces;
using Core.Mappers;
using Core.UseCase.LoginUserUseCase.Boundaries;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Core.UseCase.LoginUserUseCase;

public class LoginUser(
    IUserRepository userRepository,
    JwtTokenProvider tokenService,
    IPasswordService passwordService,
    ILogger<LoginUser> logger
) : IRequestHandler<LoginUserInput, Output>
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly JwtTokenProvider _tokenService = tokenService;
    private readonly IPasswordService _passwordService = passwordService;
    private readonly ILogger<LoginUser> _logger = logger;

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

        var token = _tokenService.GenerateToken(user.Id, user.Email);

        _logger.LogInformation("User {Email} logged in successfully.", input.Email);

        output.AddResult(user.MapToDto(token));
        return output;
    }
}
