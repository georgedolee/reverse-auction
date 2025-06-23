using FluentValidation;
using SharedInfrastructure.Extensions;

namespace AuctionService.Application.Features.Auctions.Commands.Create;

public sealed class CreateAuctionCommandValidator : AbstractValidator<CreateAuctionCommand>
{
    public CreateAuctionCommandValidator()
    {
        RuleFor(x => x.OwnerId).ValidId("OwnerId");

        RuleFor(x => x.DesiredPrice)
            .NotEmpty().WithMessage("Desired price is required.")
            .GreaterThan(0).WithMessage("Desired price must be greater than 0.");

        RuleFor(x => x.Currency)
            .NotEmpty().WithMessage("Currency is required.")
            .Matches("^[A-Z]{3}$").WithMessage("Currency must be 3 uppercase letters (ISO format).");

        RuleFor(x => x.Title)
            .MaximumLength(100).WithMessage("Title can't be longer than 100 characters.");

        RuleFor(x => x.Description)
            .MaximumLength(2000).WithMessage("Description can't be longer than 2000 characters.");

        RuleFor(x => x.StartsAt)
            .NotEmpty().WithMessage("Start time is required.")
            .Must(start => start > DateTime.UtcNow)
            .WithMessage("Start time must be in the future.");

        RuleFor(x => x.EndsAt)
            .NotEmpty().WithMessage("End time is required.")
            .Must((cmd, end) => end > cmd.StartsAt)
            .WithMessage("End time must be after start time.");
    }
}
