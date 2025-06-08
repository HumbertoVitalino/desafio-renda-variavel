using System.Text.Json.Serialization;

namespace QuoteConsumer.Worker.Dto;

public sealed record QuoteMessageDto
{
    [JsonPropertyName("ticker")]
    public string Ticker { get; init; } = string.Empty;

    [JsonPropertyName("price")]
    public decimal Price { get; init; }

    [JsonPropertyName("name")]
    public string Name { get; init; } = string.Empty;

    [JsonPropertyName("change")]
    public decimal Change { get; init; }

    [JsonPropertyName("volume")]
    public long Volume { get; init; }

    [JsonPropertyName("market_cap")]
    public long? MarketCap { get; init; }

    [JsonPropertyName("tradetime")]
    public DateTime TradeTime { get; init; }
}
