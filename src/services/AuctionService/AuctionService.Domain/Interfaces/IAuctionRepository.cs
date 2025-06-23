using AuctionService.Domain.Entities;
using SharedKernel.Interfaces;

namespace AuctionService.Domain.Interfaces;

public interface IAuctionRepository : IFullRepository<Auction>
{
}
