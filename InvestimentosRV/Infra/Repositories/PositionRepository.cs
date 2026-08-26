using Application.Interfaces.Repositories;
using Domain.Entities;
using Infra.Repositories.Mappers;
using Infra.Repositories.Models;
using Microsoft.EntityFrameworkCore;

namespace Infra.Repositories;

public class PositionRepository(AppDbContext context) : Repository<Position, PositionModel>(context), IPositionRepository
{
    protected override PositionModel ToModel(Position entity) => entity.MapToModel();
    protected override Position ToDomain(PositionModel model) => model.MapToDomain();

    public async Task<IEnumerable<Position>> GetAllByAssetIdAsync(int assetId, CancellationToken cancellationToken)
    {
        var models = await _context.Positions
            .AsNoTracking()
            .Where(p => p.AssetId == assetId && p.Quantity > 0)
            .ToListAsync(cancellationToken);

        return models.MapToDomain();
    }

    public async Task<IEnumerable<Position>> GetAllByUserIdAsync(int userId, CancellationToken cancellationToken)
    {
        var models = await _context.Positions
            .AsNoTracking()
            .Where(x => x.UserId.Equals(userId) && x.Quantity > 0)
            .Include(x => x.Asset)
            .ToListAsync(cancellationToken);

        return models.MapToDomain();
    }

    public async Task<IEnumerable<Position>> GetAllWithDetailsAsync(CancellationToken cancellationToken)
    {
        var models = await _context.Positions
            .AsNoTracking()
            .Where(p => p.Quantity > 0)
            .Include(p => p.User)
            .ToListAsync(cancellationToken);

        return models.MapToDomain();
    }

    public async Task<Position?> GetByUserIdAndAssetIdAsync(int userId, int assetId, CancellationToken cancellationToken)
    {
        var model = await _context.Positions
            .AsNoTracking()
            .Include(x => x.Asset)
            .FirstOrDefaultAsync(p => p.UserId == userId && p.AssetId == assetId, cancellationToken);

        return model?.MapToDomain();
    }
}
