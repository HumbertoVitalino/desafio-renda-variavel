using Application.Commons;
using Application.UseCases.GetTotalBrokerageRevenueUseCase.Boundaries;

namespace Application.Interfaces.UseCases;

public interface IGetTotalBrokerageRevenueUseCase
{
    Task<Output> Handle(GetTotalBrokerageRevenueInput input, CancellationToken cancellationToken);
}
