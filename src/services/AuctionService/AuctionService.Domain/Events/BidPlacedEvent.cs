using AuctionService.Domain.Entities;
using SharedKernel.DomainEvents;

namespace AuctionService.Domain.Events;

public record BidPlacedEvent(Guid Id, Guid AuctionId, Guid BidId, Guid BidderId, decimal Amount, DateTime CreatedAt)
    : DomainEvent(Id)
{
    public static BidPlacedEvent FromBid(Bid bid)
    {
        return new BidPlacedEvent(
                Id: Guid.NewGuid(),
                BidId: bid.Id,
                BidderId: bid.BidderId,
                AuctionId: bid.AuctionId,
                Amount: bid.Amount,
                CreatedAt: bid.CreatedAt);
    }
}