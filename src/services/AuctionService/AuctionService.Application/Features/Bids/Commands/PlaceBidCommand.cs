using AuctionService.Application.Contracts.Models;
using MediatR;

namespace AuctionService.Application.Features.Bids.Commands;

public sealed class PlaceBidCommand : IRequest<BidModel>
{
    public PlaceBidCommand(Guid auctionId, Guid bidderId, decimal amount)
    {
        AuctionId = auctionId;
        BidderId = bidderId;
        Amount = amount;
    }

    public Guid AuctionId { get; set; }
    public Guid BidderId { get; set; }
    public decimal Amount { get; set; }
}
