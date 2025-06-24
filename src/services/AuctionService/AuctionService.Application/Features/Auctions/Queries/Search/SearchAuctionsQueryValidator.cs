using AuctionService.Application.Contracts.Models;
using FluentValidation;
using SharedKernel.Validators;

namespace AuctionService.Application.Features.Auctions.Queries.Search;

public sealed class SearchAuctionsQueryValidator
    : PaginatedQueryValidator<SearchAuctionsQuery, AuctionModel>
{
    public SearchAuctionsQueryValidator()
    {
        RuleFor(x => x.MaxDesiredPrice)
            .GreaterThanOrEqualTo(0).When(x => x.MaxDesiredPrice.HasValue)
            .WithMessage("MaxDesiredPrice must be 0 or more.");

        RuleFor(x => x.Currency)
            .Matches("^[A-Z]{3}$")
            .WithMessage("Currency must be a 3-letter uppercase ISO code.");

        RuleFor(x => x.Title)
            .MaximumLength(100)
            .WithMessage("Title cannot be longer than 100 characters.");

        RuleFor(x => x.Description)
            .MaximumLength(2000)
            .WithMessage("Description cannot be longer than 2000 characters.");

        RuleFor(x => x.MinStartTime)
            .LessThanOrEqualTo(x => x.MaxEndTime!.Value)
            .When(x => x.MinStartTime.HasValue && x.MaxEndTime.HasValue)
            .WithMessage("MinStartTime must be earlier than or equal to MaxEndTime.");

        RuleFor(x => x.MaxCurrentBidAmount)
            .GreaterThanOrEqualTo(0).When(x => x.MaxCurrentBidAmount.HasValue)
            .WithMessage("MaxCurrentBidAmount must be 0 or more.");
    }
}
