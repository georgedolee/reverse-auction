using AuctionService.Application.Contracts.Models;
using MediatR;

namespace AuctionService.Application.Features.Auctions.Commands.Create;

internal sealed class CreateAuctionCommandHandler : IRequestHandler<CreateAuctionCommand, AuctionModel>
{
    public Task<AuctionModel> Handle(CreateAuctionCommand command, CancellationToken ct)
    {
        throw new NotImplementedException();
    }
}
