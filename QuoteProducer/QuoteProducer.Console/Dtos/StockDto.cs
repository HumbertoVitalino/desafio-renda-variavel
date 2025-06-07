using System.Text.Json.Serialization;

namespace QuoteProducer.Console.Dtos;

public sealed record StockDto
{
   [JsonPropertyName("ticker")]
   public string Ticker { get; init; } = string.Empty;

   [JsonPropertyName("price")]
   public decimal Price { get; init; }

   [JsonPropertyName("name")]
   public string Name { get; init; } = string.Empty;

   [JsonPropertyName("change")]
   public decimal Change { get; init; }

   [JsonPropertyName("closeyest")]
   public decimal YesterdayClosePrice { get; init; }

   [JsonPropertyName("volume")]
   public long Volume { get; init; }

   [JsonPropertyName("marketcap")]
   public long? MarketCap { get; init; }

   [JsonPropertyName("tradetime")]
   public DateTime TradeTime { get; init; }
}
