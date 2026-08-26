namespace Application.DTOs;

public sealed record PositionDto(
    string TickerSymbol,
    string AssetName,
    int Quantity,
    decimal AveragePrice,
    decimal CurrentProfitAndLoss
);
