using Application.DTOs;
using Domain.Entities;
using Domain.Enums;

namespace Application.Mappers;

public static class UserMapper
{
    public static User MapToDomain(string name, string email, byte[] passwordHash, byte[] passwordSalt, decimal brokerageRate, InvestorProfile profile)
    {
        return new User(
            name,
            email,
            passwordHash,
            passwordSalt,
            brokerageRate,
            profile
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
