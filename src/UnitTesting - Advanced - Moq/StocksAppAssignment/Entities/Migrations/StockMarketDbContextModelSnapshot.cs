﻿// <auto-generated />
using System;
using Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Entities.Migrations
{
    [DbContext(typeof(StockMarketDbContext))]
    partial class StockMarketDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Entities.BuyOrder", b =>
                {
                    b.Property<Guid>("BuyOrderID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("DateAndTimeOfOrder")
                        .HasColumnType("datetime2");

                    b.Property<double?>("Price")
                        .HasColumnType("float");

                    b.Property<long?>("Quantity")
                        .HasColumnType("bigint");

                    b.Property<string>("StockName")
                        .HasMaxLength(40)
                        .HasColumnType("nvarchar(40)");

                    b.Property<string>("StockSymbol")
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.HasKey("BuyOrderID");

                    b.ToTable("BuyOrders", (string)null);

                    b.HasData(
                        new
                        {
                            BuyOrderID = new Guid("c9ae2f7f-29fb-4cb0-8c59-0a07062eb104"),
                            DateAndTimeOfOrder = new DateTime(2023, 10, 22, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Price = 25.73,
                            Quantity = 96L,
                            StockName = "GGP Inc.",
                            StockSymbol = "GGP"
                        },
                        new
                        {
                            BuyOrderID = new Guid("876177c0-cca7-4bea-b21e-dce33825d91b"),
                            DateAndTimeOfOrder = new DateTime(2023, 9, 25, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Price = 15.08,
                            Quantity = 97L,
                            StockName = "Adams Natural  Resources Fund, Inc.",
                            StockSymbol = "PEO"
                        },
                        new
                        {
                            BuyOrderID = new Guid("b5ff3ba9-602c-41d2-97f2-3fcad7786422"),
                            DateAndTimeOfOrder = new DateTime(2024, 2, 3, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Price = 64.700000000000003,
                            Quantity = 62L,
                            StockName = "Star Gas Partners, L.P.",
                            StockSymbol = "SGU"
                        },
                        new
                        {
                            BuyOrderID = new Guid("a7b27a7c-4fe3-442c-9d8b-e60d6473c7a5"),
                            DateAndTimeOfOrder = new DateTime(2023, 7, 6, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Price = 3.6499999999999999,
                            Quantity = 100L,
                            StockName = "Bellatrix Exploration Ltd",
                            StockSymbol = "BXE"
                        });
                });

            modelBuilder.Entity("Entities.SellOrder", b =>
                {
                    b.Property<Guid>("SellOrderID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("DateAndTimeOfOrder")
                        .HasColumnType("datetime2");

                    b.Property<double?>("Price")
                        .HasColumnType("float");

                    b.Property<long?>("Quantity")
                        .HasColumnType("bigint");

                    b.Property<string>("StockName")
                        .HasMaxLength(40)
                        .HasColumnType("nvarchar(40)");

                    b.Property<string>("StockSymbol")
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.HasKey("SellOrderID");

                    b.ToTable("SellOrders", (string)null);

                    b.HasData(
                        new
                        {
                            SellOrderID = new Guid("ee78a812-fbf9-495e-849c-f289ce661ee4"),
                            DateAndTimeOfOrder = new DateTime(2023, 10, 22, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Price = 25.73,
                            Quantity = 45L,
                            StockName = "GGP Inc.",
                            StockSymbol = "GGP"
                        },
                        new
                        {
                            SellOrderID = new Guid("6a68f8e7-8457-41bc-92dd-89880fb0d788"),
                            DateAndTimeOfOrder = new DateTime(2023, 9, 25, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Price = 15.08,
                            Quantity = 15L,
                            StockName = "Adams Natural  Resources Fund, Inc.",
                            StockSymbol = "PEO"
                        },
                        new
                        {
                            SellOrderID = new Guid("9723bd32-86a1-4974-9a4c-5c209dad3579"),
                            DateAndTimeOfOrder = new DateTime(2024, 2, 3, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Price = 64.700000000000003,
                            Quantity = 792L,
                            StockName = "Star Gas Partners, L.P.",
                            StockSymbol = "SGU"
                        },
                        new
                        {
                            SellOrderID = new Guid("33eba596-71d8-421d-99d0-f31af0bbe439"),
                            DateAndTimeOfOrder = new DateTime(2023, 7, 6, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Price = 3.6499999999999999,
                            Quantity = 235L,
                            StockName = "Bellatrix Exploration Ltd",
                            StockSymbol = "BXE"
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
