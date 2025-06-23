using AuctionService.Domain.Entities;
using AuctionService.Domain.Interfaces;
using SharedInfrastructure.Persistance.Repositories;

namespace AuctionService.Infrastructure.Persistance.Repositories;

internal class AuctionRepository : EFFullRepository<Auction>, IAuctionRepository
{
    public AuctionRepository(AuctionDbContext context)
        : base(context)
    {
    }
}
