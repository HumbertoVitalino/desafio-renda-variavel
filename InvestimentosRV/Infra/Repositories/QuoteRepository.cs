using Application.Interfaces.Repositories;
using Domain.Entities;
using Infra.Repositories.Mappers;
using Infra.Repositories.Models;
using Microsoft.EntityFrameworkCore;

namespace Infra.Repositories;

public class QuoteRepository(AppDbContext context) : Repository<Quote, QuoteModel>(context), IQuoteRepository
{
    protected override QuoteModel ToModel(Quote entity) => entity.MapToModel();
    protected override Quote ToDomain(QuoteModel model) => model.MapToDomain();

    public async Task<Quote?> GetLatestQuoteByAssetIdAsync(int assetId, CancellationToken cancellationToken)
    {
        var model = await _context.Quotes
            .AsNoTracking()
            .Where(x => x.AssetId.Equals(assetId))
            .OrderByDescending(x => x.DateTime)
            .FirstOrDefaultAsync(cancellationToken);

        return model?.MapToDomain();
    }

    public async Task<Dictionary<int, decimal>> GetLatestQuotesForAllAssetsAsync(CancellationToken cancellationToken)
    {
        return await _context.Quotes
            .AsNoTracking()
            .GroupBy(x => x.AssetId)
            .Select(g => g.OrderByDescending(x => x.DateTime).First())
            .ToDictionaryAsync(q => q.AssetId, q => q.UnitPrice, cancellationToken);
    }

    public async Task<Quote?> GetByAssetIdAndTradeTimeAsync(int assetId, DateTime tradeTime, CancellationToken cancellationToken)
    {
        var model = await _context.Quotes
            .AsNoTracking()
            .FirstOrDefaultAsync(q => q.AssetId == assetId && q.DateTime == tradeTime, cancellationToken);

        return model?.MapToDomain();
    }
}
