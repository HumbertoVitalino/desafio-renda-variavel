namespace Core.Dtos;

public sealed record PositionDto(
    string TickerSymbol,
    string AssetName,
    int Quantity,
    decimal AveragePrice,
    decimal CurrentProfitAndLoss
);
