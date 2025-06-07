using Core.Domain;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infra.Repositories;

public class QuoteRepository(AppDbContext context) : Repository<Quote>(context), IQuoteRepository
{
    public async Task<Quote?> GetLatestQuoteByAssetIdAsync(int assetId, CancellationToken cancellationToken)
    {
        return await _context.Quotes
            .Where(x => x.AssetId.Equals(assetId))
            .OrderByDescending(x => x.DateTime)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<Dictionary<int, decimal>> GetLatestQuotesForAllAssetsAsync(CancellationToken cancellationToken)
    {
        return await _context.Quotes
            .GroupBy(x => x.AssetId)
            .Select(g => g.OrderByDescending(x => x.DateTime).First())
            .ToDictionaryAsync(q => q.AssetId, q => q.UnitPrice, cancellationToken);
    }
}
