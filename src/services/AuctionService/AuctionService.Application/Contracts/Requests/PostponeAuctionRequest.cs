namespace AuctionService.Application.Contracts.Requests;

public class PostponeAuctionRequest
{
    public DateTime StartTime { get; set; }

    public DateTime EndTime { get; set; }
}
