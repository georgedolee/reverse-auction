using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Constants;
using SharedKernel.Responses;
using SharedKernel.Results;
using UserService.Application.Contracts.Models;
using UserService.Application.Features.Roles.Commands.Create;
using UserService.Application.Features.Roles.Commands.Update;
using UserService.Application.Features.Roles.Commands.Delete;
using UserService.Application.Features.Roles.Queries.GetById;
using UserService.Application.Features.Roles.Queries.GetPaginated;
using UserService.Application.Contracts.Requests;

namespace UserService.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(Policy = RolePolicies.AdminsOnly)]
[ProducesResponseType(StatusCodes.Status401Unauthorized)]
[ProducesResponseType(StatusCodes.Status403Forbidden)]
[ProducesResponseType(StatusCodes.Status500InternalServerError)]
public class RolesController : ControllerBase
{
    private readonly IMediator _mediator;

    public RolesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateRoleAsync(
        [FromBody] CreateRoleCommand command, CancellationToken ct)
    {
        var response = await _mediator.Send(command, ct);

        return CreatedAtAction(
            nameof(GetRoleByIdAsync),
            new { id = response.Id },
            ApiResponse<RoleModel>
                .Ok("New role created successfully.", response));
    }

    [HttpGet("{id:guid}")]
    [ActionName(nameof(GetRoleByIdAsync))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetRoleByIdAsync(
        [FromRoute] Guid id, CancellationToken ct)
    {
        var query = new GetRoleByIdQuery(id);
        var response = await _mediator.Send(query, ct);

        return Ok(ApiResponse<RoleModel>
            .Ok($"Role with id {id} fetched successfully.", response));
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetPaginatedRolesAsync(
        [FromQuery] GetPaginatedRolesQuery query, CancellationToken ct)
    {
        var response = await _mediator.Send(query, ct);

        return Ok(ApiResponse<PagedResult<RoleModel>>
            .Ok(
                $"Roles from page {query.PageNumber} with page size {query.PageSize} fetched successfully",
                response)
            );
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateRoleAsync(
        [FromRoute] Guid id,
        [FromBody] UpdateRoleRequest request,
        CancellationToken ct)
    {
        var command = new UpdateRoleCommand(id: id, name: request.Name);
        var response = await _mediator.Send(command, ct);

        return Ok(ApiResponse<RoleModel>
            .Ok($"User  with id {id} updated successfully.", response));
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeleteRoleAsync(
        [FromRoute] Guid id, CancellationToken ct)
    {
        var command = new DeleteRoleCommand(id);
        _ = await _mediator.Send(command, ct);

        return NoContent();
    }
}
