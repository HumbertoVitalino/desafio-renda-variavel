using Core.Domain;
using Core.Dtos;

namespace Core.Mappers;

public static class QuoteMapper
{
    public static QuoteDto MapToDto(this Quote quote)
    {
        return new QuoteDto(
            quote.Asset.TickerSymbol,
            quote.Asset.Name,
            quote.UnitPrice,
            quote.DateTime
        );
    }
}
