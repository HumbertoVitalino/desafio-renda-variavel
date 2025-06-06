using Core.Domain;
using Core.Interfaces;

namespace Infra.Repositories;

public class PositionRepositoy(AppDbContext context) : Repository<Position>(context), IPositionRepository
{
}
