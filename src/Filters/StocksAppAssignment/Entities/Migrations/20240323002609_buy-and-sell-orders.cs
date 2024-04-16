using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Entities.Migrations
{
    /// <inheritdoc />
    public partial class buyandsellorders : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BuyOrders",
                columns: table => new
                {
                    BuyOrderID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StockSymbol = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    StockName = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    DateAndTimeOfOrder = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Quantity = table.Column<long>(type: "bigint", nullable: true),
                    Price = table.Column<double>(type: "float", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BuyOrders", x => x.BuyOrderID);
                });

            migrationBuilder.CreateTable(
                name: "SellOrders",
                columns: table => new
                {
                    SellOrderID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StockSymbol = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    StockName = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    DateAndTimeOfOrder = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Quantity = table.Column<long>(type: "bigint", nullable: true),
                    Price = table.Column<double>(type: "float", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SellOrders", x => x.SellOrderID);
                });

            migrationBuilder.InsertData(
                table: "BuyOrders",
                columns: new[] { "BuyOrderID", "DateAndTimeOfOrder", "Price", "Quantity", "StockName", "StockSymbol" },
                values: new object[,]
                {
                    { new Guid("876177c0-cca7-4bea-b21e-dce33825d91b"), new DateTime(2023, 9, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), 15.08, 97L, "Adams Natural  Resources Fund, Inc.", "PEO" },
                    { new Guid("a7b27a7c-4fe3-442c-9d8b-e60d6473c7a5"), new DateTime(2023, 7, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), 3.6499999999999999, 100L, "Bellatrix Exploration Ltd", "BXE" },
                    { new Guid("b5ff3ba9-602c-41d2-97f2-3fcad7786422"), new DateTime(2024, 2, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), 64.700000000000003, 62L, "Star Gas Partners, L.P.", "SGU" },
                    { new Guid("c9ae2f7f-29fb-4cb0-8c59-0a07062eb104"), new DateTime(2023, 10, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), 25.73, 96L, "GGP Inc.", "GGP" }
                });

            migrationBuilder.InsertData(
                table: "SellOrders",
                columns: new[] { "SellOrderID", "DateAndTimeOfOrder", "Price", "Quantity", "StockName", "StockSymbol" },
                values: new object[,]
                {
                    { new Guid("33eba596-71d8-421d-99d0-f31af0bbe439"), new DateTime(2023, 7, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), 3.6499999999999999, 235L, "Bellatrix Exploration Ltd", "BXE" },
                    { new Guid("6a68f8e7-8457-41bc-92dd-89880fb0d788"), new DateTime(2023, 9, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), 15.08, 15L, "Adams Natural  Resources Fund, Inc.", "PEO" },
                    { new Guid("9723bd32-86a1-4974-9a4c-5c209dad3579"), new DateTime(2024, 2, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), 64.700000000000003, 792L, "Star Gas Partners, L.P.", "SGU" },
                    { new Guid("ee78a812-fbf9-495e-849c-f289ce661ee4"), new DateTime(2023, 10, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), 25.73, 45L, "GGP Inc.", "GGP" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BuyOrders");

            migrationBuilder.DropTable(
                name: "SellOrders");
        }
    }
}
