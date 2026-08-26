using Domain.Entities;

namespace Application.Interfaces.Repositories;

public interface IPositionRepository : IRepository<Position>
{
    Task<Position?> GetByUserIdAndAssetIdAsync(int userId, int assetId, CancellationToken cancellationToken);
    Task<IEnumerable<Position>> GetAllByUserIdAsync(int userId, CancellationToken cancellationToken);
    Task<IEnumerable<Position>> GetAllWithDetailsAsync(CancellationToken cancellationToken);
    Task<IEnumerable<Position>> GetAllByAssetIdAsync(int assetId, CancellationToken cancellationToken);
}
