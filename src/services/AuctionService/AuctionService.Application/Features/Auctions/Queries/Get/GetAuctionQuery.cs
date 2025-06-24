using AuctionService.Application.Contracts.Models;
using MediatR;

namespace AuctionService.Application.Features.Auctions.Queries.Get;

public sealed class GetAuctionQuery : IRequest<AuctionModel>
{
    public GetAuctionQuery(Guid id)
    {
        Id = id;
    }

    public Guid Id { get; }
}
