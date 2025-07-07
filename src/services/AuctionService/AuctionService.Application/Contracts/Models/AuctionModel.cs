using AuctionService.Domain.Entities;

namespace AuctionService.Application.Contracts.Models;

public class AuctionModel
{
    public Guid Id { get; set; }
    public Guid OwnerId { get; set; }

    public required string Title { get; set; }
    public required string Description { get; set; }
    public string? Image { get; set; }
    public decimal DesiredPrice { get; set; }

    public DateTime StartsAt { get; set; }
    public DateTime EndsAt { get; set; }

    public required string Status { get; set; }

    public Guid? LowestBidderId { get; set; }
    public decimal? LowestBidAmount { get; set; }

    public IEnumerable<Bid>? Bids { get; set; }

    public DateTime CreatedAt { get; set; }
}
