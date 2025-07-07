using AuctionService.Application.Contracts.Models;
using MediatR;
using SharedKernel.Queries;

namespace AuctionService.Application.Features.Bids.Queries.Search;

public sealed class SearchBidsQuery : PaginatedQuery<BidModel>
{
    public Guid? AuctionId { get; set; }

    public Guid? BidderId { get; set; }

    public decimal? MaxAmount { get; set; }

    public decimal? MinAmount { get; set; }
}
