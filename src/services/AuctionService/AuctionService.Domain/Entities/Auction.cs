using AuctionService.Domain.Enums;
using AuctionService.Domain.Events;
using SharedKernel.Entities;

namespace AuctionService.Domain.Entities;

public class Auction
    : EntityWithDomainEvents
{
    public Guid OwnerId { get; set; }

    public required string Title { get; set; }
    public required string Description { get; set; }
    public decimal DesiredPrice { get; set; }
    public required string Currency { get; set; }

    public DateTime StartsAt { get; set; }
    public DateTime EndsAt { get; set; }

    public AuctionStatus Status { get; set; }

    public Guid? LowestBidderId { get; set; }
    public decimal? LowestBidAmount { get; set; }
    public ICollection<Bid> Bids { get; set; } = [];
    

    public void Postpone(DateTime startTime, DateTime endTime)
    {
        if (Status != AuctionStatus.Pending)
        {
            throw new InvalidOperationException("Only pending auctions can be postponed.");
        }

        StartsAt = startTime.AddSeconds(5);
        EndsAt = endTime;

        RaiseDomainEvent(new AuctionPostponedEvent(Guid.NewGuid(), Id, startTime, endTime));
    }

    public void Start()
    {
        if (Status != AuctionStatus.Pending)
        {
            throw new InvalidOperationException("Only pending auctions can be started manually.");
        }

        StartsAt = DateTime.UtcNow.AddSeconds(5);
        Status = AuctionStatus.Ongoing;

        RaiseDomainEvent(new AuctionStartedEvent(Guid.NewGuid(), Id, StartsAt));
    }

    public void End()
    {
        if (Status != AuctionStatus.Ongoing)
        {
            throw new InvalidOperationException("Only active auctions can be ended manually.");
        }

        EndsAt = DateTime.UtcNow;
        Status = AuctionStatus.Ended;

        RaiseDomainEvent(new AuctionEndedEvent(Guid.NewGuid(), Id, EndsAt));
    }

    public void Cancel()
    {
        if (Status == AuctionStatus.Ended)
        {
            throw new InvalidOperationException("Cannot cancel an auction that has already ended.");
        }

        Status = AuctionStatus.Cancelled;

        RaiseDomainEvent(new AuctionCancelledEvent(Guid.NewGuid(), Id, DateTime.UtcNow));
    }

    public void PlaceBid(Bid bid)
    {
        if (Status != AuctionStatus.Ongoing)
        {
            throw new InvalidOperationException("Auction is not currently active.");
        }

        var now = DateTime.UtcNow;
        if (now < StartsAt || now > EndsAt)
        {
            throw new InvalidOperationException("Auction is not running at this time.");
        }

        if (LowestBidAmount.HasValue && LowestBidderId == bid.BidderId)
        {
            throw new InvalidOperationException("You are already the highest bidder.");
        }

        var acceptableBid = LowestBidAmount.HasValue
            ? LowestBidAmount.Value
            : DesiredPrice;

        if (bid.Amount >= acceptableBid)
        {
            throw new InvalidOperationException($"Bid amount must be at least {acceptableBid}.");
        }

        LowestBidAmount = bid.Amount;
        LowestBidderId = bid.BidderId;
        Bids.Add(bid);

        RaiseDomainEvent(BidPlacedEvent.FromBid(bid));
    }
}
