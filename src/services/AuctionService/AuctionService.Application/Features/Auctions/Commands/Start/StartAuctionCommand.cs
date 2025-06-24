using MediatR;

namespace AuctionService.Application.Features.Auctions.Commands.Start;

public sealed class StartAuctionCommand : IRequest<bool>
{
    public StartAuctionCommand(Guid auctionId, Guid userId)
    {
        AuctionId = auctionId;
        UserId = userId;
    }

    public Guid AuctionId { get; set; }
    public Guid UserId { get; set; }
}
