using Api.Mappers;
using Api.Requests;
using Core.Commons;
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

    [HttpPost("register")]
    [ProducesResponseType(typeof(Output), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Output), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> RegisterAsync([FromBody] NewUserRequest request, CancellationToken cancellationToken)
    {
        var input = request.MapToInput();

        var output = await _mediator.Send(input, cancellationToken);

        if (!output.IsValid)
            return BadRequest(output);

        return Ok(output);
    }

    [HttpPost("login")]
    [ProducesResponseType(typeof(Output), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Output), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> LoginAsync([FromBody] LoginUserRequest request, CancellationToken cancellationToken)
    {
        var input = request.MapToInput();

        var output = await _mediator.Send(input, cancellationToken);

        if (!output.IsValid)
            return Unauthorized(output);

        return Ok(output);
    }
}
