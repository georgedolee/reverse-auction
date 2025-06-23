using AuctionService.Domain.Entities;
using AuctionService.Domain.Interfaces;
using SharedInfrastructure.Persistance.Repositories;

namespace AuctionService.Infrastructure.Persistance.Repositories;

internal class BidRepository : EFFullRepository<Bid>, IBidRepository
{
    public BidRepository(AuctionDbContext context)
        : base(context)
    {
    }
}
