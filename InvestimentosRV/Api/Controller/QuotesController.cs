using Api.Requests;
using Core.Commons;
using Core.UseCase.GetLatestQuoteUseCase.Boundaries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controller;

[ApiController]
[Route("api/v1/[controller]")]
[Authorize]
public class QuotesController(
    IMediator mediator    
) : BaseController
{
    private readonly IMediator _mediator = mediator;

    [HttpGet("{tickerSymbol}/latest")]
    [ProducesResponseType(typeof(Output), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Output), StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetLatestQuoteAsync([FromRoute] string tickerSymbol, CancellationToken cancellationToken)
    {
        GetLatestQuoteInput input = new(tickerSymbol); 

        var output = await _mediator.Send(input, cancellationToken);

        if (!output.IsValid)
            return NotFound(output);

        return Ok(output);
    }
}
