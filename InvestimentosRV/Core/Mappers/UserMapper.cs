using Core.Domain;
using Core.Dtos;
using Core.UseCase.NewUserUseCase.Boundaries;

namespace Core.Mappers;

public static class UserMapper
{
    private const decimal DefaultBrokerageRate = 0;

    public static User MapToDomain(this NewUserInput input, byte[] passwordHash, byte[] passwordSalt)
    {
        return new User(
            input.Name,
            input.Email,
            passwordHash,
            passwordSalt,
            DefaultBrokerageRate
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
}
