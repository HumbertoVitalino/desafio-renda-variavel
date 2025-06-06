namespace Core.Dtos;

public sealed record UserDto(
    string Name,
    string Email,
    decimal BrokerageRate
);

public sealed record UserLoginDto(
    int Id,
    string Name,
    string Email,
    string Token
);
