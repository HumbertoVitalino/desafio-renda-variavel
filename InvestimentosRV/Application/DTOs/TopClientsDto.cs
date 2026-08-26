namespace Application.DTOs;

public sealed record TopClientsDto(
    int UserId,
    string UserName,
    decimal TotalPositionValue
);
