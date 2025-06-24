using AuctionService.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using SharedKernel.Guards;

namespace AuctionService.Application.Features.Auctions.Commands.End;

internal sealed class EndAuctionCommandHandler : IRequestHandler<EndAuctionCommand, bool>
{
    private readonly IAuctionServiceUnitOfWork _unitOfWork;
    private readonly ILogger<EndAuctionCommand> _logger;

    public EndAuctionCommandHandler(
        IAuctionServiceUnitOfWork unitOfWork,
        ILogger<EndAuctionCommand> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<bool> Handle(EndAuctionCommand command, CancellationToken ct)
    {
        _logger.LogInformation("Trying to manually end auction with id: {auctionId}.", command.AuctionId);
        var auction = await _unitOfWork.Auctions.GetAsync(command.AuctionId, ct);

        Guard.EnsureFound(auction, nameof(auction), command.AuctionId, _logger);
        Guard.EnsureUserOwnsResource(auction!.OwnerId, command.UserId, nameof(auction), _logger);

        auction.End();
        await _unitOfWork.CommitAsync(ct);

        _logger.LogInformation("Auction with id: {auctionId} successfully ended.", command.AuctionId);
        return true;
    }
}
