namespace Core.Dtos;

public sealed record QuoteDto(
    string TickerSymbol,
    string AssetName,
    decimal UnitPrice,
    DateTime DateTime
);
