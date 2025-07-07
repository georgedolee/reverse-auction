using AuctionService.Application.Contracts.Models;
using AuctionService.Domain.Entities;
using AuctionService.Domain.Interfaces;
using Mapster;
using MediatR;
using Microsoft.Extensions.Logging;
using SharedKernel.Guards;

namespace AuctionService.Application.Features.Bids.Commands;

internal sealed class PlaceBidCommandHandler : IRequestHandler<PlaceBidCommand, BidModel>
{
    private readonly IAuctionServiceUnitOfWork _unitOfWork;
    private readonly ILogger<PlaceBidCommandHandler> _logger;
    public PlaceBidCommandHandler(
        IAuctionServiceUnitOfWork unitOfWork,
        ILogger<PlaceBidCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<BidModel> Handle(PlaceBidCommand command, CancellationToken ct)
    {
        _logger.LogInformation("Trying to place bid with properties: {@bid}", command);
        var auction = await _unitOfWork.Auctions.GetAsync(command.AuctionId, ct);

        Guard.EnsureFound(auction, nameof(auction), command.AuctionId, _logger);

        var newBid = command.Adapt<Bid>();

        auction!.PlaceBid(newBid);
        await _unitOfWork.Bids.AddAsync(newBid, ct);
        await _unitOfWork.CommitAsync(ct);

        _logger.LogInformation("Bid placed successfully.");
        return newBid.Adapt<BidModel>();
    }
}
