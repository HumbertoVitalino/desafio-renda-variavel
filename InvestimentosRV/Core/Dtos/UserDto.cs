namespace Core.Dtos;

public sealed record UserDto(
    string Name,
    string Email,
    decimal BrokerageRate
);
