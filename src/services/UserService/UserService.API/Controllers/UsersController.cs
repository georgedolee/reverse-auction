using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Constants;
using SharedKernel.Exceptions;
using SharedKernel.Guards;
using SharedKernel.Responses;
using SharedKernel.Results;
using System.Security.Claims;
using UserService.Application.Contracts.Models;
using UserService.Application.Contracts.Requests;
using UserService.Application.Features.Users.Commands.AddToRole;
using UserService.Application.Features.Users.Commands.Delete;
using UserService.Application.Features.Users.Commands.DeletePhoto;
using UserService.Application.Features.Users.Commands.RemoveFromRole;
using UserService.Application.Features.Users.Commands.Update;
using UserService.Application.Features.Users.Commands.UploadPhoto;
using UserService.Application.Features.Users.Queries.GetById;
using UserService.Application.Features.Users.Queries.GetPaginated;

namespace UserService.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[ProducesResponseType(StatusCodes.Status401Unauthorized)]
[ProducesResponseType(StatusCodes.Status403Forbidden)]
[ProducesResponseType(StatusCodes.Status500InternalServerError)]
public class UsersController : ControllerBase
{
    private readonly IMediator _mediator;

    public UsersController(IMediator mediator)
    {
        _mediator = mediator;        
    }

    [HttpGet("{id:guid}")]
    [ActionName(nameof(GetUserByIdAsync))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetUserByIdAsync(
        [FromRoute] Guid id, CancellationToken ct)
    {
        var query = new GetUserByIdQuery(id);
        var response = await _mediator.Send(query, ct);

        return Ok(ApiResponse<UserModel>
            .Ok($"User with id {id} fetched successfully.", response));
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetPaginatedUsersAsync(
        [FromQuery] GetPaginatedUsersQuery query, CancellationToken ct)
    {
        var response = await _mediator.Send(query, ct);

        return Ok(ApiResponse<PagedResult<UserModel>>
            .Ok(
                $"Users from page {query.PageNumber} with page size {query.PageSize} fetched successfully", 
                response)
            );
    }

    [HttpPut("{id:guid}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateUserAsync(
        [FromRoute] Guid id,
        [FromBody] UpdateUserRequest request, 
        CancellationToken ct)
    {
        EnsureUserOwnsResource(id);

        var command = new UpdateUserCommand(
            id: id,
            userName: request.UserName,
            email: request.Email);

        var response = await _mediator.Send(command, ct);

        return Ok(ApiResponse<UserModel>
            .Ok($"User  with id {id} updated successfully.", response));
    }

    [HttpDelete("{id:guid}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeleteUserAsync(
        [FromRoute] Guid id, CancellationToken ct)
    {
        EnsureUserOwnsResource(id);

        var command = new DeleteUserCommand(id);
        _ = await _mediator.Send(command, ct);

        return NoContent();
    }

    [HttpPost("{userId:guid}/roles/{roleName}")]
    [Authorize(Policy = RolePolicies.AdminsOnly)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AddToRole(
        [FromRoute] AddUserToRoleCommand command,
        CancellationToken ct)
    {
        _ = await _mediator.Send(command, ct);

        return Ok(ApiResponse<object?>
            .Ok($"User with id {command.UserId} successfully added to the role {command.RoleName}"));
    }

    [HttpDelete("{userId:guid}/roles/{roleName}")]
    [Authorize(Policy = RolePolicies.AdminsOnly)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> RemoveFromRole(
        [FromRoute] RemoveFromRoleCommand command,
        CancellationToken ct)
    {
        _ = await _mediator.Send(command, ct);

        return NoContent();
    }

    [HttpPost("{id:guid}/upload-photo")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UploadUserPhotoAsync(
        [FromRoute] Guid id,
        IFormFile file, 
        CancellationToken ct)
    {
        EnsureUserOwnsResource(id);

        if (file is null || file.Length == 0)
        {
            return BadRequest("No file provided.");
        }

        var command = new UploadUserPhotoCommand(id, file);
        _ = await _mediator.Send(command, ct);

        return NoContent();
    }

    [HttpDelete("{id:guid}/delete-photo")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeleteUserPhotoAsync(
        [FromRoute] Guid id,
        CancellationToken ct)
    {
        EnsureUserOwnsResource(id);

        var command = new DeleteUserPhotoCommand(id);
        _ = await _mediator.Send(command, ct);

        return NoContent();
    }

    private void EnsureUserOwnsResource(Guid id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId != id.ToString())
        {
            throw new ForbiddenException();
        }
    }
}
