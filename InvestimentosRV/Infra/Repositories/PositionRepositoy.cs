using Core.Domain;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infra.Repositories;

public class PositionRepositoy(AppDbContext context) : Repository<Position>(context), IPositionRepository
{
    public Task<Position?> GetByUserIdAndAssetIdAsync(int userId, int assetId, CancellationToken cancellationToken)
    {
        return _context.Set<Position>()
            .FirstOrDefaultAsync(p => p.UserId == userId && p.AssetId == assetId, cancellationToken);
    }
}
