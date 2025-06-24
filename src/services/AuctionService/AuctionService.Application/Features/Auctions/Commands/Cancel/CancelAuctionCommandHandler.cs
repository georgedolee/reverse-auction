using AuctionService.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using SharedKernel.Guards;

namespace AuctionService.Application.Features.Auctions.Commands.Cancel;

internal sealed class CancelAuctionCommandHandler : IRequestHandler<CancelAuctionCommand, bool>
{
    private readonly IAuctionServiceUnitOfWork _unitOfWork;
    private readonly ILogger<CancelAuctionCommandHandler> _logger;

    public CancelAuctionCommandHandler(
        IAuctionServiceUnitOfWork unitOfWork,
        ILogger<CancelAuctionCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<bool> Handle(CancelAuctionCommand command, CancellationToken ct)
    {
        _logger.LogInformation("Trying to cancel an auction with id: {auctionId}", command.AuctionId);
        var auction = await _unitOfWork.Auctions.GetAsync(command.AuctionId, ct);

        Guard.EnsureFound(auction, nameof(auction), command.AuctionId, _logger);
        Guard.EnsureUserOwnsResource(auction!.OwnerId, command.UserId, nameof(auction), _logger);

        auction.Cancel();
        await _unitOfWork.CommitAsync(ct);

        _logger.LogInformation("Auction with id: {auctionId} cancelled successfully.", command.AuctionId);
        return true;
    }
}
