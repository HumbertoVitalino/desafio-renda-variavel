using Domain.Entities;

namespace Application.Interfaces.Repositories;

public interface IAssetRepository : IRepository<Asset>
{
    Task<Asset?> GetByTickerAsync(string tickerSymbol, CancellationToken cancellationToken);
    Task<IEnumerable<Asset>> GetAllAssetsAsync(CancellationToken cancellationToken);
}
