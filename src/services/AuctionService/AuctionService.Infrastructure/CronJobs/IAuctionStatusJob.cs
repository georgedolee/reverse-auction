namespace AuctionService.Infrastructure.CronJobs;

public interface IAuctionStatusJob
{
    Task StartAuctionsAsync();

    Task EndAuctionsAsync();
}
