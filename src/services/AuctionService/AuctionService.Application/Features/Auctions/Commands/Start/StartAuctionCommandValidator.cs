using FluentValidation;
using SharedInfrastructure.Extensions;

namespace AuctionService.Application.Features.Auctions.Commands.Start;

public sealed class StartAuctionCommandValidator : AbstractValidator<StartAuctionCommand>
{
    public StartAuctionCommandValidator()
    {
        RuleFor(x => x.AuctionId).ValidId("AuctionId is required.");

        RuleFor(x => x.UserId).ValidId("AuctionId is required.");
    }
}
