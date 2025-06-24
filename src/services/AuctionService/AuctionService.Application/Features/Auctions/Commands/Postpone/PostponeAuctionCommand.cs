using AuctionService.Application.Contracts.Requests;
using MediatR;

namespace AuctionService.Application.Features.Auctions.Commands.Postpone;

public sealed class PostponeAuctionCommand : PostponeAuctionRequest, IRequest<bool>
{
    public PostponeAuctionCommand(
        Guid auctionId, 
        Guid userId,
        DateTime startTime,
        DateTime endTime)
    {
        AuctionId = auctionId;
        UserId = userId;
        StartTime = startTime;
        EndTime = endTime;
    }

    public Guid AuctionId { get; set; }
    public Guid UserId { get; set; }
}
