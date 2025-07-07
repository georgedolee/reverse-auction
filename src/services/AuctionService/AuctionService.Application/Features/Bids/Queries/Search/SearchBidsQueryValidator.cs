using AuctionService.Application.Contracts.Models;
using FluentValidation;
using SharedKernel.Validators;

namespace AuctionService.Application.Features.Bids.Queries.Search;

public sealed class SearchBidsQueryValidator
    : PaginatedQueryValidator<SearchBidsQuery, BidModel>
{
    public SearchBidsQueryValidator()
    {
        RuleFor(x => x.MaxAmount)
            .GreaterThanOrEqualTo(0)
            .When(x => x.MaxAmount.HasValue)
            .WithMessage("MaxAmount must be 0 or more.");

        RuleFor(x => x.MinAmount)
            .GreaterThanOrEqualTo(0)
            .When(x => x.MinAmount.HasValue)
            .WithMessage("MinAmount must be 0 or more.");

        RuleFor(x => x)
            .Must(x => x.MinAmount <= x.MaxAmount)
            .When(x => x.MinAmount.HasValue && x.MaxAmount.HasValue)
            .WithMessage("MinAmount must be less than or equal to MaxAmount.");
    }
}
