namespace AuctionService.Infrastructure.Settings;

public class CronSettings
{
    public string StartAuctions { get; set; } = "* * * * *";

    public string EndAuctions { get; set; } = "* * * * *";
}