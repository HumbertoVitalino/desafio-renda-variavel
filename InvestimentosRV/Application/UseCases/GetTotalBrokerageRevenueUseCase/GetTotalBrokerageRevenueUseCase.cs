using Application.Commons;
using Application.DTOs;
using Application.Interfaces.Repositories;
using Application.Interfaces.UseCases;
using Application.UseCases.GetTotalBrokerageRevenueUseCase.Boundaries;

namespace Application.UseCases.GetTotalBrokerageRevenueUseCase;

public class GetTotalBrokerageRevenueUseCase(
    IOperationRepository operationRepository
) : IGetTotalBrokerageRevenueUseCase
{
    private readonly IOperationRepository _operationRepository = operationRepository;

    public async Task<Output> Handle(GetTotalBrokerageRevenueInput input, CancellationToken cancellationToken)
    {
        Output output = new();

        var totalRevenue = await _operationRepository.GetTotalBrokerageRevenueAsync(cancellationToken);
        output.AddResult(new BrokerageRevenueDto(totalRevenue));
        return output;
    }
}
