﻿using Core.Domain;

namespace Core.Interfaces;

public interface IOperationRepository : IRepository<Operation>
{
    Task<decimal> GetTotalBrokerageRevenueAsync(CancellationToken cancellationToken);
}
