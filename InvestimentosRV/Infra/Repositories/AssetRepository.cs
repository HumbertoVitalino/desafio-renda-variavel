using Application.Interfaces.Repositories;
using Domain.Entities;
using Infra.Repositories.Mappers;
using Infra.Repositories.Models;
using Microsoft.EntityFrameworkCore;

namespace Infra.Repositories;

public class AssetRepository(AppDbContext context) : Repository<Asset, AssetModel>(context), IAssetRepository
{
    protected override AssetModel ToModel(Asset entity) => entity.MapToModel();
    protected override Asset ToDomain(AssetModel model) => model.MapToDomain();

    public async Task<Asset?> GetByTickerAsync(string tickerSymbol, CancellationToken cancellationToken)
    {
        var model = await _context.Assets
            .AsNoTracking()
            .FirstOrDefaultAsync(a => a.TickerSymbol.Equals(tickerSymbol), cancellationToken);

        return model?.MapToDomain();
    }

    public async Task<IEnumerable<Asset>> GetAllAssetsAsync(CancellationToken cancellationToken)
    {
        var models = await _context.Assets
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        return models.MapToDomain();
    }
}
