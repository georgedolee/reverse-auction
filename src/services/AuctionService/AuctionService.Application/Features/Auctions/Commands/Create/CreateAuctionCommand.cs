using AuctionService.Application.Contracts.Models;
using AuctionService.Application.Contracts.Requests;
using MediatR;

namespace AuctionService.Application.Features.Auctions.Commands.Create;

public sealed class CreateAuctionCommand : CreateAuctionRequest, IRequest<AuctionModel>
{
    public CreateAuctionCommand(
        Guid ownerId,
        string title,
        string description,
        decimal desiredPrice,
        string currency,
        DateTime startsAt,
        DateTime endsAt)
    {
        OwnerId = ownerId;
        Title = title;
        Description = description;
        DesiredPrice = desiredPrice;
        Currency = currency;
        StartsAt = startsAt;
        EndsAt = endsAt;
    }

    public Guid OwnerId { get; set; }
}
