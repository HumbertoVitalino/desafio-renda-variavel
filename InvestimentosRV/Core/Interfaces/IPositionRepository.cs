using Core.Domain;

namespace Core.Interfaces;

public interface IPositionRepository : IRepository<Position>
{
    Task<Position?> GetByUserIdAndAssetIdAsync(int userId, int assetId, CancellationToken cancellationToken);
}
