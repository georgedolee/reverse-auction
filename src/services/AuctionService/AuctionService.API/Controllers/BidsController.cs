using AuctionService.Application.Contracts.Models;
using AuctionService.Application.Contracts.Requests;
using AuctionService.Application.Features.Bids.Commands;
using AuctionService.Application.Features.Bids.Queries.Get;
using AuctionService.Application.Features.Bids.Queries.Search;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedInfrastructure.Extensions;
using SharedKernel.Responses;
using SharedKernel.Results;

namespace AuctionService.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[ProducesResponseType(StatusCodes.Status401Unauthorized)]
[ProducesResponseType(StatusCodes.Status403Forbidden)]
[ProducesResponseType(StatusCodes.Status429TooManyRequests)]
[ProducesResponseType(StatusCodes.Status500InternalServerError)]
public class BidsController : ControllerBase
{
    private readonly IMediator _mediator;

    public BidsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{id:guid}")]
    [Authorize]
    [ActionName(nameof(GetBidByIdAsync))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetBidByIdAsync(
        [FromRoute] Guid id, CancellationToken ct)
    {
        var query = new GetBidQuery(id);

        var response = await _mediator.Send(query, ct);

        return Ok(ApiResponse<BidModel>
            .Ok($"Bid with id: {id} fetched successfully.", response));
    }

    [HttpGet("search")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetBidsByAuctionIdAsync(
        [FromQuery] SearchBidsQuery query, CancellationToken ct)
    {
        var response = await _mediator.Send(query, ct);

        return Ok(ApiResponse<PagedResult<BidModel>>
            .Ok($"Bids fetched successfully.", response));
    }

    [HttpPost]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> PlaceBidAsync(
        [FromBody] PlaceBidRequest request, CancellationToken ct)
    {
        var userId = User.GetUserId();
        var command = new PlaceBidCommand(
            auctionId: request.AuctionId,
            bidderId: userId,
            amount: request.Amount);

        var response = await _mediator.Send(command, ct);

        return CreatedAtAction(
                nameof(GetBidByIdAsync),
                new { id = response.Id },
                ApiResponse<BidModel>.Ok("Bid placed successfully.", response)
        );
    }
}
