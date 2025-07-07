using FluentValidation;
using SharedInfrastructure.Extensions;

namespace AuctionService.Application.Features.Bids.Commands;

public sealed class PlaceBidCommandValidator : AbstractValidator<PlaceBidCommand>
{
    public PlaceBidCommandValidator()
    {
        RuleFor(x => x.AuctionId).ValidId("Auction Id");

        RuleFor(x => x.BidderId).ValidId("Bidder Id");

        RuleFor(x => x.Amount)
            .NotEmpty().WithMessage("Amount is required.")
            .GreaterThan(0).WithMessage("Bid amount must be greater than 0.");
    }
}

