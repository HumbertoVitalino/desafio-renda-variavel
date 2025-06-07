using Core.Commons;
using Core.UseCase.GetAllUserPositionsUseCase.Boundaries;
using Core.UseCase.GetUserAssetPositionUseCase.Boundaries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controller;

[ApiController]
[Route("api/v1/[controller]")]
[Authorize]
public class PositionsController(
    IMediator mediator    
) : BaseController
{
    private readonly IMediator _mediator = mediator;

    [HttpGet("{tickerSymbol}")]
    [ProducesResponseType(typeof(Output), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Output), StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetUserAssetPositionAsync([FromRoute] string tickerSymbol, CancellationToken cancellationToken)
    {
        GetUserAssetPositionInput input = new(UserId, tickerSymbol);

        var output = await _mediator.Send(input, cancellationToken);

        if (!output.IsValid)
            return NotFound(output);

        return Ok(output);
    }

    [HttpGet]
    [ProducesResponseType(typeof(Output), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Output), StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAllUserPositionsAsync(CancellationToken cancellationToken)
    {
        var input = new GetAllUserPositionsInput(UserId);
        var output = await _mediator.Send(input, cancellationToken);

        if (!output.IsValid)
            return NotFound(output);

        return Ok(output);
    }
}
