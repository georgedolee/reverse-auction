using AuctionService.Domain.Interfaces;
using AuctionService.Infrastructure.CronJobs;
using AuctionService.Infrastructure.Persistance;
using AuctionService.Infrastructure.Persistance.UnitOfWork;
using AuctionService.Infrastructure.Scheduling;
using AuctionService.Infrastructure.Settings;
using Hangfire;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AuctionService.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var defaultConnection = configuration.GetConnectionString("DefaultConnection");

        services.AddDbContext<AuctionDbContext>(options =>
            options.UseSqlServer(defaultConnection)
        );

        services.Configure<CronSettings>(configuration.GetSection("CronSettings"));

        services.AddHangfire(config =>
        {
            config.UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseSqlServerStorage(defaultConnection);
        }).AddHangfireServer();

        services.AddScoped<IAuctionStatusJob, AuctionStatusJob>();
        services.AddSingleton<IJobScheduler, HangfireJobScheduler>();

        services.AddScoped<IAuctionServiceUnitOfWork, AuctionServiceUnitOfWork>();

        return services;
    }
}