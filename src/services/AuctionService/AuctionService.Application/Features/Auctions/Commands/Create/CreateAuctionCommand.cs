using AuctionService.Application.Contracts.Models;
using MediatR;

namespace AuctionService.Application.Features.Auctions.Commands.Create;

public sealed class CreateAuctionCommand : IRequest<AuctionModel>
{
    public Guid OwnerId { get; set; }

    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal DesiredPrice { get; set; }
    public string Currency { get; set; } = string.Empty;

    public DateTime StartsAt { get; set; }
    public DateTime EndsAt { get; set; }
}
