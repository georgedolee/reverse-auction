using AuctionService.Application.Contracts.Models;
using AuctionService.Domain.Enums;
using SharedKernel.Queries;

namespace AuctionService.Application.Features.Auctions.Queries.Search;

public sealed class SearchAuctionsQuery : PaginatedQuery<AuctionModel>
{
    public Guid? OwnerId { get; set; }

    public string? Title { get; set; }
    public string? Description { get; set; }

    public decimal? MaxDesiredPrice { get; set; }
    public string? Currency { get; set; }

    public DateTime? MinStartTime { get; set; }
    public DateTime? MaxEndTime { get; set; }

    public decimal? MaxCurrentBidAmount { get; set; }

    public AuctionStatus? Status { get; set; }
}
