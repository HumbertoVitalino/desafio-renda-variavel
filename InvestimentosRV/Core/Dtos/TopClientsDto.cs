namespace Core.Dtos;

public sealed record TopClientsDto(
    int UserId,
    string UserName,
    decimal TotalPositionValue
);
