using Core.Domain;
using Core.Dtos;
using Core.UseCase.NewUserUseCase.Boundaries;

namespace Core.Mappers;

public static class UserMapper
{
    public static User MapToDomain(this NewUserInput input, byte[] passwordHash, byte[] passwordSalt, decimal brokerageRate)
    {
        return new User(
            input.Name,
            input.Email,
            passwordHash,
            passwordSalt,
            brokerageRate,
            input.Profile
        );
    }    

    public static UserDto MapToDto(this User user)
    {
        return new UserDto(
            user.Name,
            user.Email,
            user.BrokerageRate
        );
    }

    public static UserLoginDto MapToDto(this User user, string token)
    {
        return new UserLoginDto(
            user.Id,
            user.Name,
            user.Email,
            token
        );
    }
}
