using Core.Domain;
using Core.Interfaces;

namespace Infra.Repositories;

public class AssetRepository(AppDbContext context) : Repository<Asset>(context), IAssetRepository
{
}
