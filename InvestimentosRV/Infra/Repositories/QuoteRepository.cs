using Core.Domain;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infra.Repositories;

public class QuoteRepository(AppDbContext context) : Repository<Quote>(context), IQuoteRepository
{
    public async Task<Quote?> GetByAssetIdAsync(int assetId, CancellationToken cancellationToken)
    {
        return await _context.Quotes
            .FirstOrDefaultAsync(x => x.AssetId.Equals(assetId), cancellationToken);
    }
}
