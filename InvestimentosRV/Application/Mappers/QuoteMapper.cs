using Application.DTOs;
using Domain.Entities;

namespace Application.Mappers;

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
