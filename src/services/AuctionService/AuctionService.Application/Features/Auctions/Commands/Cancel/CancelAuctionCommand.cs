using MediatR;

namespace AuctionService.Application.Features.Auctions.Commands.Cancel;

public sealed class CancelAuctionCommand : IRequest<bool>
{
    public CancelAuctionCommand(Guid auctionId, Guid userId)
    {
        AuctionId = auctionId;
        UserId = userId;
    }

    public Guid AuctionId { get; set; }
    public Guid UserId { get; set; }
}
