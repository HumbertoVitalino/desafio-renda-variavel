using Application.Interfaces.Repositories;
using Domain.Entities;
using Infra.Repositories.Mappers;
using Infra.Repositories.Models;
using Microsoft.EntityFrameworkCore;

namespace Infra.Repositories;

public class OperationRepository(AppDbContext context) : Repository<Operation, OperationModel>(context), IOperationRepository
{
    protected override OperationModel ToModel(Operation entity) => entity.MapToModel();
    protected override Operation ToDomain(OperationModel model) => model.MapToDomain();

    public async Task<decimal> GetTotalBrokerageRevenueAsync(CancellationToken cancellationToken)
    {
        return await _context.Operations.SumAsync(x => x.BrokerageFee, cancellationToken);
    }
}
