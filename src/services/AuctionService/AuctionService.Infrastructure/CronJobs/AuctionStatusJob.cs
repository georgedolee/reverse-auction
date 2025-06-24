using AuctionService.Domain.Enums;
using AuctionService.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace AuctionService.Infrastructure.CronJobs;

internal class AuctionStatusJob : IAuctionStatusJob
{
    private readonly IAuctionServiceUnitOfWork _unitOfWork;
    private readonly ILogger<AuctionStatusJob> _logger;

    public AuctionStatusJob(
        IAuctionServiceUnitOfWork unitOfWork,
        ILogger<AuctionStatusJob> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task EndAuctionsAsync()
    {
        _logger.LogInformation("ending expired auctions.");

        var now = DateTime.UtcNow;
        var expiredAuctions = await _unitOfWork.Auctions.FindAsync(auction =>
            auction.EndsAt <= now &&
            auction.Status == AuctionStatus.Ongoing
        );

        if (!expiredAuctions.Any())
        {
            _logger.LogInformation("No expired auctions to end at {Timestamp}.", now);
            return;
        }

        foreach (var auction in expiredAuctions)
        {
            try
            {
                auction.End();
                _logger.LogInformation("Auction {AuctionId} ended.", auction.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to end auction {AuctionId}.", auction.Id);
            }
        }

        await _unitOfWork.CommitAsync();
        _logger.LogInformation("Expired auctions committed successfully.");
    }

    public async Task StartAuctionsAsync()
    {
        _logger.LogInformation("Starting upcoming auctions.");

        var now = DateTime.UtcNow;
        var upcomingAuctions = await _unitOfWork.Auctions.FindAsync(auction =>
            auction.StartsAt <= now && auction.Status == AuctionStatus.Pending
        );

        if (!upcomingAuctions.Any())
        {
            _logger.LogInformation("No auctions to start at {Timestamp}.", now);
            return;
        }

        foreach (var auction in upcomingAuctions)
        {
            try
            {
                auction.Start();
                _logger.LogInformation("Auction {AuctionId} started.", auction.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to start auction {AuctionId}.", auction.Id);
            }
        }

        await _unitOfWork.CommitAsync();
        _logger.LogInformation("Upcoming auctions committed successfully.");
    }
}
