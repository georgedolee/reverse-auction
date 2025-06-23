using AuctionService.Domain.Interfaces;
using AuctionService.Infrastructure.Persistance.Repositories;
using SharedInfrastructure.Persistance.UnitOfWork;

namespace AuctionService.Infrastructure.Persistance.UnitOfWork;

public class AuctionServiceUnitOfWork : EFUnitOfWork<AuctionDbContext>, IAuctionServiceUnitOfWork
{
    public AuctionServiceUnitOfWork(AuctionDbContext context)
        : base(context)
    {
        Auctions = new AuctionRepository(context);
        Bids = new BidRepository(context);
    }

    public IAuctionRepository Auctions { get; }
    public IBidRepository Bids { get; }
}
