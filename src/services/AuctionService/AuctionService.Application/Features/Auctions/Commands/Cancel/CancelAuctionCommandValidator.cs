using FluentValidation;
using SharedInfrastructure.Extensions;

namespace AuctionService.Application.Features.Auctions.Commands.Cancel;

public sealed class CancelAuctionCommandValidator : AbstractValidator<CancelAuctionCommand>
{
    public CancelAuctionCommandValidator()
    {
        RuleFor(x => x.AuctionId).ValidId("AuctionId");

        RuleFor(x => x.UserId).ValidId("UserId");
    }
}
