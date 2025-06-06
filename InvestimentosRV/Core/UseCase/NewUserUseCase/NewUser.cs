using Core.Commons;
using Core.Interfaces;
using Core.Mappers;
using Core.UseCase.NewUserUseCase.Boundaries;
using MediatR;

namespace Core.UseCase.NewUserUseCase;

public class NewUser(
    IUserRepository userRepository,
    IPasswordService passwordService,
    IUnitOfWork unitOfWork
) : IRequestHandler<NewUserInput, Output>
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IPasswordService _passwordService = passwordService;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Output> Handle(NewUserInput input, CancellationToken cancellationToken)
    {
        Output output = new();

        var existingUser = await _userRepository.GetByEmailAsync(input.Email, cancellationToken);

        if (existingUser is not null)
        {
            output.AddErrorMessage("This email already exists.");
            return output;
        }
        
        if (input.Password != input.Confirmation)
        {
            output.AddErrorMessage("Password and confirmation do not match.");
            return output;
        }

        var (passwordHash, passwordSalt) = _passwordService.CreatePasswordHash(input.Password);

        var user = input.MapToDomain(passwordHash, passwordSalt);

        await _userRepository.CreateAsync(user, cancellationToken);
        await _unitOfWork.CommitAsync(cancellationToken);

        output.AddResult(user.MapToDto());
        return output;
    }
}
