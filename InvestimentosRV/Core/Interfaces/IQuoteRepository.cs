using Core.Domain;

namespace Core.Interfaces;

public interface IQuoteRepository : IRepository<Quote>
{
    Task<Quote?> GetByAssetIdAsync(int assetId, CancellationToken cancellationToken);
}
