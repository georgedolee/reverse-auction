﻿// <auto-generated />
using System;
using AuctionService.Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace AuctionService.Infrastructure.Migrations
{
    [DbContext(typeof(AuctionDbContext))]
    [Migration("20250707102945_AuctionImage")]
    partial class AuctionImage
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("AuctionService.Domain.Entities.Auction", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Currency")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("DesiredPrice")
                        .HasColumnType("decimal(11,2)");

                    b.Property<DateTime>("EndsAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Image")
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal?>("LowestBidAmount")
                        .HasColumnType("decimal(11,2)");

                    b.Property<Guid?>("LowestBidderId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("OwnerId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("StartsAt")
                        .HasColumnType("datetime2");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Auctions", null, t =>
                        {
                            t.HasCheckConstraint("CK_Auctions_Currency_ISO", "LEN(Currency) = 3 AND Currency = UPPER(Currency)");

                            t.HasCheckConstraint("CK_Auctions_EndsAt_After_StartsAt", "EndsAt > StartsAt");

                            t.HasCheckConstraint("CK_Auctions_StartsAt_InFuture", "StartsAt >= GETUTCDATE()");
                        });
                });

            modelBuilder.Entity("AuctionService.Domain.Entities.Bid", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<decimal>("Amount")
                        .HasColumnType("decimal(11,2)");

                    b.Property<Guid>("AuctionId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("BidderId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("AuctionId");

                    b.ToTable("Bids", null, t =>
                        {
                            t.HasCheckConstraint("CK_Bids_Amount_Positive", "Amount > 0");
                        });
                });

            modelBuilder.Entity("AuctionService.Domain.Entities.Bid", b =>
                {
                    b.HasOne("AuctionService.Domain.Entities.Auction", null)
                        .WithMany("Bids")
                        .HasForeignKey("AuctionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("AuctionService.Domain.Entities.Auction", b =>
                {
                    b.Navigation("Bids");
                });
#pragma warning restore 612, 618
        }
    }
}
