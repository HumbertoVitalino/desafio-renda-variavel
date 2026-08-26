using Application.DTOs;
using Domain.Entities;

namespace Application.Mappers;

public static class AssetMapper
{
    public static AssetDto MapToDto(this Asset asset)
    {
        return new AssetDto(
            asset.TickerSymbol,
            asset.Name,
            asset.Risk
        );
    }

    public static IEnumerable<AssetDto> MapToDto(this IEnumerable<Asset> assets)
    {
        return assets.Select(asset => asset.MapToDto());
    }
}
