using AuctionService.Application.Contracts.Models;
using SharedKernel.Validators;

namespace AuctionService.Application.Features.Auctions.Queries.GetPaginated;

public sealed class GetAuctionsPaginatedQueryValidator
    : PaginatedQueryValidator<GetAuctionsPaginatedQuery, AuctionModel>
{
}
