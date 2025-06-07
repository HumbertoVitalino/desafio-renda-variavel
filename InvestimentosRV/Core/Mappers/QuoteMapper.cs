using Core.Domain;
using Core.Dtos;

namespace Core.Mappers;

public static class QuoteMapper
{
    public static QuoteDto MapToDto(this Quote quote)
    {
        return new QuoteDto(
            quote.Asset.Name,
            quote.Asset.TickerSymbol,
            quote.UnitPrice,
            quote.DateTime
        );
    }
}
