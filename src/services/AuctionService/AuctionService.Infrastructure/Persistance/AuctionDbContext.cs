using AuctionService.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SharedKernel.DomainEvents;

namespace AuctionService.Infrastructure.Persistance;

public class AuctionDbContext : DbContext
{
    private readonly IPublisher _publisher;

    public AuctionDbContext(
        DbContextOptions<AuctionDbContext> options,
        IPublisher publisher)
        : base(options)
    {
        _publisher = publisher;
    }

   public DbSet<Auction> Auctions { get; set; }
    public DbSet<Bid> Bids { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AuctionDbContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken ct = default)
    {
        var domainEvents = ChangeTracker.Entries<IHasDomainEvents>()
            .Select(e => e.Entity)
            .Where(e => e.DomainEvents.Any())
            .SelectMany(e => e.DomainEvents)
            .ToList();

        var result = await base.SaveChangesAsync(ct);

        foreach (var domainEvent in domainEvents)
        {
            await _publisher.Publish(domainEvent, ct);
        }

        return result;
    }
}
