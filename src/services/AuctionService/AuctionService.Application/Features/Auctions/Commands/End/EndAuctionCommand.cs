using MediatR;

namespace AuctionService.Application.Features.Auctions.Commands.End;

public sealed class EndAuctionCommand : IRequest<bool>
{
    public EndAuctionCommand(Guid auctionId, Guid userId)
    {
        AuctionId = auctionId;
        UserId = userId;
    }

    public Guid AuctionId { get; set; }
    public Guid UserId { get; set; }
}
