using Domain.Enums;

namespace Application.DTOs;

public sealed record AssetDto(
    string TickerSymbol,
    string Name,
    AssetRisk Risk
);
