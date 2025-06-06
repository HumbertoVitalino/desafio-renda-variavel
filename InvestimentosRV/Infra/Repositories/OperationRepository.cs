using Core.Domain;
using Core.Interfaces;

namespace Infra.Repositories;

public class OperationRepository(AppDbContext context) : Repository<Operation>(context), IOperationRepository
{
}
