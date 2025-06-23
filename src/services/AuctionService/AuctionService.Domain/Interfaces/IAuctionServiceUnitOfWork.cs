using SharedKernel.Interfaces;

namespace AuctionService.Domain.Interfaces;

public interface IAuctionServiceUnitOfWork : IUnitOfWork
{
    IAuctionRepository Auctions { get; }
    IBidRepository Bids { get; }
}
