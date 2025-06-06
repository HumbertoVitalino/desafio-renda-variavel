using Core.Domain;

namespace Core.Interfaces;

public interface IAssetRepository : IRepository<Asset>
{
    Task<Asset?> GetByTickerAsync(string tickerSymbol, CancellationToken cancellationToken);
}
