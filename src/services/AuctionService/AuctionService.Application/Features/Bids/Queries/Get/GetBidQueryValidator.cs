using FluentValidation;
using SharedInfrastructure.Extensions;

namespace AuctionService.Application.Features.Bids.Queries.Get;

public sealed class GetBidQueryValidator : AbstractValidator<GetBidQuery>
{
    public GetBidQueryValidator()
    {
        RuleFor(x => x.Id).ValidId();
    }
}
