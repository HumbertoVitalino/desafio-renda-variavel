using Core.Domain;

namespace Core.Interfaces;

public interface IQuoteRepository : IRepository<Quote>
{
    Task<Quote?> GetLatestQuoteByAssetIdAsync(int assetId, CancellationToken cancellationToken);
    Task<Quote?> GetByAssetIdAndTradeTimeAsync(int assetId, DateTime tradeTime, CancellationToken cancellationToken);
    Task<Dictionary<int, decimal>> GetLatestQuotesForAllAssetsAsync(CancellationToken cancellationToken);
}
