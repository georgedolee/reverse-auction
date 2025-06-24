using AuctionService.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using SharedKernel.Guards;

namespace AuctionService.Application.Features.Auctions.Commands.Start;

internal sealed class StartAuctionCommandHandler : IRequestHandler<StartAuctionCommand, bool>
{
    private readonly IAuctionServiceUnitOfWork _unitOfWork;
    private readonly ILogger<StartAuctionCommandHandler> _logger;

    public StartAuctionCommandHandler(
        IAuctionServiceUnitOfWork unitOfWork,
        ILogger<StartAuctionCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<bool> Handle(StartAuctionCommand command, CancellationToken ct)
    {
        _logger.LogInformation("Tryin to manually start auction with id: {auctionId}", command.AuctionId);
        var auction = await _unitOfWork.Auctions.GetAsync(command.AuctionId, ct);

        Guard.EnsureFound(auction, nameof(auction), command.AuctionId, _logger);
        Guard.EnsureUserOwnsResource(auction!.OwnerId, command.UserId, nameof(auction), _logger);

        auction.Start();
        await _unitOfWork.CommitAsync(ct);

        _logger.LogInformation("Auction with id: {auctionId} started successfully.", command.AuctionId);
        return true;
    }
}