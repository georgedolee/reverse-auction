using AuctionService.Application.Contracts.Models;
using AuctionService.Application.Contracts.Requests;
using AuctionService.Application.Features.Auctions.Commands.Cancel;
using AuctionService.Application.Features.Auctions.Commands.Create;
using AuctionService.Application.Features.Auctions.Commands.End;
using AuctionService.Application.Features.Auctions.Commands.Postpone;
using AuctionService.Application.Features.Auctions.Commands.Start;
using AuctionService.Application.Features.Auctions.Queries.Get;
using AuctionService.Application.Features.Auctions.Queries.GetPaginated;
using AuctionService.Application.Features.Auctions.Queries.Search;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Responses;
using SharedKernel.Results;
using System.Security.Claims;

namespace AuctionService.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[ProducesResponseType(StatusCodes.Status401Unauthorized)]
[ProducesResponseType(StatusCodes.Status403Forbidden)]
[ProducesResponseType(StatusCodes.Status429TooManyRequests)]
[ProducesResponseType(StatusCodes.Status500InternalServerError)]
public class AuctionsController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuctionsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AddAuctionAsync(
        [FromBody] CreateAuctionCommand command, CancellationToken ct)
    {
        var response = await _mediator.Send(command, ct);

        return CreatedAtAction(
                nameof(GetAuctionByIdAsync),
                new { id = response.Id },
                ApiResponse<AuctionModel>
                    .Ok("Auction Created successfully.", response)
        );
    }

    [HttpGet("{id:guid}")]
    [Authorize]
    [ActionName(nameof(GetAuctionByIdAsync))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAuctionByIdAsync(
        [FromRoute] Guid id, CancellationToken ct)
    {
        var query = new GetAuctionQuery(id);

        var response = await _mediator.Send(query, ct);

        return Ok(ApiResponse<AuctionModel>
            .Ok($"Auction with id: {id} fetched successfully.", response));
    }

    [HttpGet]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAuctionsPaginatedAsync(
        [FromQuery] GetAuctionsPaginatedQuery query, CancellationToken ct)
    {
        var response = await _mediator.Send(query, ct);

        return Ok(ApiResponse<PagedResult<AuctionModel>>
            .Ok("All auctions fetched successfully", response));
    }

    [HttpGet("search")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> SearchAuctionAsync(
        [FromQuery] SearchAuctionsQuery query, CancellationToken ct)
    {
        var response = await _mediator.Send(query, ct);

        return Ok(ApiResponse<PagedResult<AuctionModel>>
            .Ok($"Auctions fetched successfully.", response));
    }

    [HttpPost("{id:guid}/start")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> StartAuctionAsync(
        [FromRoute] Guid id, CancellationToken ct)
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        _ = await _mediator.Send(new StartAuctionCommand(id, userId), ct);

        return Ok(ApiResponse<object?>
            .Ok($"Auction with id: {id} started successfully."));
    }

    [HttpPost("{id:guid}/end")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> EndAuctionAsync(
        [FromRoute] Guid id, CancellationToken ct)
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        _ = await _mediator.Send(new EndAuctionCommand(id, userId), ct);

        return Ok(ApiResponse<object?>
            .Ok($"Auction with id: {id} ended successfully."));
    }

    [HttpPost("{id:guid}/cancel")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CancelAuctionAsync(
        [FromRoute] Guid id, CancellationToken ct)
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        _ = await _mediator.Send(new CancelAuctionCommand(id, userId), ct);

        return Ok(ApiResponse<object?>
            .Ok($"Auction with id: {id} cancelled successfully."));
    }

    [HttpPost("{id:guid}/postpone")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> PostponeAuctionAsync(
        [FromRoute] Guid id,
        [FromBody] PostponeAuctionRequest request,
        CancellationToken ct)
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        var command = new PostponeAuctionCommand(
            auctionId: id,
            userId: userId,
            startTime: request.StartTime,
            endTime: request.EndTime);

        _ = await _mediator.Send(command, ct);

        return Ok(ApiResponse<object?>
            .Ok($"Auction with id: {id} postponed successfully."));
    }
}