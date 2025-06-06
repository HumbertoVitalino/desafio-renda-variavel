using Core.Domain;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infra.Repositories;

public class AssetRepository(AppDbContext context) : Repository<Asset>(context), IAssetRepository
{
    public async Task<Asset?> GetByTickerAsync(string tickerSymbol, CancellationToken cancellationToken)
    {
        return await _context.Set<Asset>()
            .AsNoTracking()
            .FirstOrDefaultAsync(a => a.TickerSymbol.Equals(tickerSymbol), cancellationToken);
    }
}
