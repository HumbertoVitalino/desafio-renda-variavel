using Domain.Enums;

namespace Application.DTOs;

public sealed record OperationDto(
    int Id,
    string TickerSymbol,
    OperationType Type,
    int Quantity,
    decimal UnitPrice,
    decimal BrokerageFee,
    DateTime DateTime
);
