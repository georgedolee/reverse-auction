using AuctionService.Application.Contracts.Models;
using AuctionService.Application.Features.Auctions.Commands.Create;
using AuctionService.Domain.Entities;
using AuctionService.Domain.Enums;
using Mapster;

namespace AuctionService.Application.Mapping;

internal sealed class AuctionMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<CreateAuctionCommand, Auction>()
            .Map(dest => dest.OwnerId, src => src.OwnerId)
            .Map(dest => dest.Title, src => src.Title)
            .Map(dest => dest.Description, src => src.Description)
            .Map(dest => dest.DesiredPrice, src => src.DesiredPrice)
            .Map(dest => dest.Currency, src => src.Currency)
            .Map(dest => dest.StartsAt, src => src.StartsAt.AddSeconds(5))
            .Map(dest => dest.EndsAt, src => src.EndsAt)
            .Map(dest => dest.CreatedAt, _ => DateTime.UtcNow)
            .Map(dest => dest.Status, _ => AuctionStatus.Pending)
            .Ignore(dest => dest.Id)
            .Ignore(dest => dest.Image!)
            .Ignore(dest => dest.Bids!)
            .Ignore(dest => dest.LowestBidAmount!)
            .Ignore(dest => dest.LowestBidderId!);

        config.NewConfig<Auction, AuctionModel>()
            .Map(dest => dest.Status, src => src.Status.ToString())
            .Map(dest => dest.Bids, src => src.Bids);
    }
}
