using AuctionService.Application.Contracts.Models;
using MediatR;

namespace AuctionService.Application.Features.Bids.Queries.Get;

public sealed class GetBidQuery : IRequest<BidModel>
{
    public GetBidQuery(Guid id)
    {
        Id = id;
    }

    public Guid Id { get; set; }
}
