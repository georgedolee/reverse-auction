using AuctionService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AuctionService.Infrastructure.Persistance.Configurations;

internal sealed class BidConfiguration : IEntityTypeConfiguration<Bid>
{
    public void Configure(EntityTypeBuilder<Bid> builder)
    {
        builder.ToTable("Bids", tb =>
        {
            tb.HasCheckConstraint(
                "CK_Bids_Amount_Positive",
                "Amount > 0"
            );
        });

        builder.HasKey(b => b.Id);

        builder.Property(b => b.AuctionId)
            .IsRequired();

        builder.Property(b => b.BidderId)
            .IsRequired();

        builder.Property(b => b.Amount)
            .HasColumnType("decimal(11,2)")
            .IsRequired();

        builder.Property(b => b.CreatedAt)
            .IsRequired();
    }
}
