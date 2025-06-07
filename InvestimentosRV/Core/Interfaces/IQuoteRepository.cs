using Core.Domain;

namespace Core.Interfaces;

public interface IQuoteRepository : IRepository<Quote>
{
    Task<Quote?> GetLatestQuoteByAssetIdAsync(int assetId, CancellationToken cancellationToken);
}
