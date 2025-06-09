using Core.Domain.Enums;

namespace Core.Dtos;

public sealed record OperationDto(
    int Id,
    string TickerSymbol,
    OperationType Type,
    int Quantity,
    decimal UnitPrice,
    decimal BrokerageFee,
    DateTime DateTime
);
