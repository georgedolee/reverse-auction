using SharedKernel.Entities;

namespace AuctionService.Domain.Entities;

public class Bid : BaseEntity
{
    public Guid AuctionId { get; set; }
    public Guid BidderId { get; set; }
    public decimal Amount { get; set; }
}
