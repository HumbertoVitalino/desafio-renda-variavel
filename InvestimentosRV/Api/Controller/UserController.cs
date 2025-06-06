using Api.Mappers;
using Api.Requests;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controller;

[ApiController]
[Route("api/v1/[controller]")]
public class UserController(
    IMediator mediator    
) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> RegisterAsync([FromBody] NewUserRequest request, CancellationToken cancellationToken)
    {
        var input = request.MapToInput();

        var output = await _mediator.Send(input, cancellationToken);

        if (!output.IsValid)
            return BadRequest(output);

        return Ok(output);
    }
}
