using Core.Commons;
using Core.Interfaces;
using Core.UseCase.GetTopClientsByPositionValueUseCase.Boundaries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controller;

[ApiController]
[Route("api/v1/[controller]")]
[Authorize]
public class ReportsController(
    IMediator mediator,
    IOperationRepository repository
) : BaseController
{
    private readonly IMediator _mediator = mediator;
    private readonly IOperationRepository _repository = repository;

    [HttpGet("brokerage-revenue")]
    [ProducesResponseType(typeof(Output), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetTotalBrokerageRevenueAsync(CancellationToken cancellationToken)
    {
        Output output = new();
        var totalRevenue = await _repository.GetTotalBrokerageRevenueAsync(cancellationToken);

        output.AddResult(new { TotalRevenue = totalRevenue });
        return Ok(output);
    }

    [HttpGet("top-10/by-position-value")]
    [ProducesResponseType(typeof(Output), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetTop10ByPositionValueAsync(CancellationToken cancellationToken)
    {
        var output = await _mediator.Send(new GetTopClientsByPositionValueInput(), cancellationToken);
        return Ok(output);
    }
}
