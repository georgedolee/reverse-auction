using FluentValidation;
using SharedInfrastructure.Extensions;

namespace AuctionService.Application.Features.Auctions.Queries.Get;

public sealed class GetAuctionQueryValidator : AbstractValidator<GetAuctionQuery>
{
    public GetAuctionQueryValidator()
    {
        RuleFor(x => x.Id).ValidId();
    }
}
