using AuctionService.Application.Contracts.Models;
using SharedKernel.Queries;

namespace AuctionService.Application.Features.Auctions.Queries.GetPaginated;

public sealed class GetAuctionsPaginatedQuery : PaginatedQuery<AuctionModel>
{
}
