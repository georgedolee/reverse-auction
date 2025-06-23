using AuctionService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AuctionService.Infrastructure.Persistance.Configurations;

internal sealed class AuctionConfiguration : IEntityTypeConfiguration<Auction>
{
    public void Configure(EntityTypeBuilder<Auction> builder)
    {
        builder.ToTable("Auctions", tb =>
        {
            tb.HasCheckConstraint(
                "CK_Auctions_StartsAt_InFuture",
                "StartsAt > GETUTCDATE()");

            tb.HasCheckConstraint(
                "CK_Auctions_EndsAt_After_StartsAt",
                "EndsAt > StartsAt");

            tb.HasCheckConstraint(
            "CK_Auctions_Currency_ISO",
            "LEN(Currency) = 3 AND Currency = UPPER(Currency)"
        );
        });

        builder.HasKey(a => a.Id);

        builder.Property(a => a.DesiredPrice)
            .HasColumnType("decimal(11,2)")
            .IsRequired();

        builder.Property(a => a.Currency)
            .IsRequired();

        builder.Property(a => a.StartsAt)
            .IsRequired();

        builder.Property(a => a.EndsAt)
            .IsRequired();

        builder.Property(a => a.LowestBidAmount)
            .HasColumnType("decimal(11,2)");

        builder.Property(a => a.Status)
            .HasConversion<int>()
            .IsRequired();

        builder.HasMany(a => a.Bids)
            .WithOne()
            .HasForeignKey(b => b.AuctionId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
