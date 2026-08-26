using Domain.Entities;

namespace Application.Interfaces.Repositories;

public interface IOperationRepository : IRepository<Operation>
{
    Task<decimal> GetTotalBrokerageRevenueAsync(CancellationToken cancellationToken);
}
