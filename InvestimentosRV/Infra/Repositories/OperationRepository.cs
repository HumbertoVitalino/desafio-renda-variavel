using Core.Domain;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infra.Repositories;

public class OperationRepository(AppDbContext context) : Repository<Operation>(context), IOperationRepository
{
    public async Task<decimal> GetTotalBrokerageRevenueAsync(CancellationToken cancellationToken)
    {
        return await _context.Operations.SumAsync(x => x.BrokerageFee, cancellationToken);
    }
}
