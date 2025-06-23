using MediatR;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Responses;
using UserService.Application.Features.Users.Commands.Register;

namespace UserService.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("register")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> RegisterUserAsync(
        [FromBody] RegisterUserCommand command, CancellationToken ct)
    {
        _ = await _mediator.Send(command, ct);

        return Ok(ApiResponse<object?>
                .Ok("Registration finished successfully."));
    }
}
