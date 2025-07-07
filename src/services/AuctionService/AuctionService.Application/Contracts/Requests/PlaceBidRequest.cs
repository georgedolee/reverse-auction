namespace AuctionService.Application.Contracts.Requests;

public sealed class PlaceBidRequest
{
    public Guid AuctionId { get; set; }

    public decimal Amount { get; set; }
}
