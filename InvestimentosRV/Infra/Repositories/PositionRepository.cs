using Core.Domain;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infra.Repositories;

public class PositionRepository(AppDbContext context) : Repository<Position>(context), IPositionRepository
{
    public async Task<IEnumerable<Position>> GetAllByUserIdAsync(int userId, CancellationToken cancellationToken)
    {
        return await _context.Positions
            .Where(x => x.UserId.Equals(userId) && x.Quantity > 0)
            .Include(x => x.Asset)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Position>> GetAllWithDetailsAsync(CancellationToken cancellationToken)
    {
        return await _context.Positions
            .Where(p => p.Quantity > 0)
            .Include(p => p.User)
            .ToListAsync(cancellationToken);
    }

    public async Task<Position?> GetByUserIdAndAssetIdAsync(int userId, int assetId, CancellationToken cancellationToken)
    {
        return await _context.Positions
            .Include(x => x.Asset)
            .FirstOrDefaultAsync(p => p.UserId == userId && p.AssetId == assetId, cancellationToken);
    }
}
