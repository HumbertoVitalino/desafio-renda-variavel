using Core.Domain.Enums;

namespace Api.Requests;

public sealed record NewOperationRequest(
    string TickerSymbol,
    int Quantity,
    OperationType Type
);
