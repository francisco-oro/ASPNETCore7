using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Entities.Migrations
{
    /// <inheritdoc />
    public partial class TIN_Updated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TIN",
                table: "People",
                newName: "TaxIdentificationNumber");

            migrationBuilder.AlterColumn<string>(
                name: "TaxIdentificationNumber",
                table: "People",
                type: "varchar(8)",
                nullable: true,
                defaultValue: "ABC12345",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "People",
                keyColumn: "PersonID",
                keyValue: new Guid("012107df-862f-4f16-ba94-e5c16886f005"),
                column: "TaxIdentificationNumber",
                value: "ABC12345");

            migrationBuilder.UpdateData(
                table: "People",
                keyColumn: "PersonID",
                keyValue: new Guid("28d11936-9466-4a4b-b9c5-2f0a8e0cbde9"),
                column: "TaxIdentificationNumber",
                value: "ABC12345");

            migrationBuilder.UpdateData(
                table: "People",
                keyColumn: "PersonID",
                keyValue: new Guid("29339209-63f5-492f-8459-754943c74abf"),
                column: "TaxIdentificationNumber",
                value: "ABC12345");

            migrationBuilder.UpdateData(
                table: "People",
                keyColumn: "PersonID",
                keyValue: new Guid("2a6d3738-9def-43ac-9279-0310edc7ceca"),
                column: "TaxIdentificationNumber",
                value: "ABC12345");

            migrationBuilder.UpdateData(
                table: "People",
                keyColumn: "PersonID",
                keyValue: new Guid("89e5f445-d89f-4e12-94e0-5ad5b235d704"),
                column: "TaxIdentificationNumber",
                value: "ABC12345");

            migrationBuilder.UpdateData(
                table: "People",
                keyColumn: "PersonID",
                keyValue: new Guid("a3b9833b-8a4d-43e9-8690-61e08df81a9a"),
                column: "TaxIdentificationNumber",
                value: "ABC12345");

            migrationBuilder.UpdateData(
                table: "People",
                keyColumn: "PersonID",
                keyValue: new Guid("ac660a73-b0b7-4340-abc1-a914257a6189"),
                column: "TaxIdentificationNumber",
                value: "ABC12345");

            migrationBuilder.UpdateData(
                table: "People",
                keyColumn: "PersonID",
                keyValue: new Guid("c03bbe45-9aeb-4d24-99e0-4743016ffce9"),
                column: "TaxIdentificationNumber",
                value: "ABC12345");

            migrationBuilder.UpdateData(
                table: "People",
                keyColumn: "PersonID",
                keyValue: new Guid("c3abddbd-cf50-41d2-b6c4-cc7d5a750928"),
                column: "TaxIdentificationNumber",
                value: "ABC12345");

            migrationBuilder.UpdateData(
                table: "People",
                keyColumn: "PersonID",
                keyValue: new Guid("c6d50a47-f7e6-4482-8be0-4ddfc057fa6e"),
                column: "TaxIdentificationNumber",
                value: "ABC12345");

            migrationBuilder.UpdateData(
                table: "People",
                keyColumn: "PersonID",
                keyValue: new Guid("cb035f22-e7cf-4907-bd07-91cfee5240f3"),
                column: "TaxIdentificationNumber",
                value: "ABC12345");

            migrationBuilder.UpdateData(
                table: "People",
                keyColumn: "PersonID",
                keyValue: new Guid("d15c6d9f-70b4-48c5-afd3-e71261f1a9be"),
                column: "TaxIdentificationNumber",
                value: "ABC12345");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TaxIdentificationNumber",
                table: "People",
                newName: "TIN");

            migrationBuilder.AlterColumn<string>(
                name: "TIN",
                table: "People",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(8)",
                oldNullable: true,
                oldDefaultValue: "ABC12345");

            migrationBuilder.UpdateData(
                table: "People",
                keyColumn: "PersonID",
                keyValue: new Guid("012107df-862f-4f16-ba94-e5c16886f005"),
                column: "TIN",
                value: null);

            migrationBuilder.UpdateData(
                table: "People",
                keyColumn: "PersonID",
                keyValue: new Guid("28d11936-9466-4a4b-b9c5-2f0a8e0cbde9"),
                column: "TIN",
                value: null);

            migrationBuilder.UpdateData(
                table: "People",
                keyColumn: "PersonID",
                keyValue: new Guid("29339209-63f5-492f-8459-754943c74abf"),
                column: "TIN",
                value: null);

            migrationBuilder.UpdateData(
                table: "People",
                keyColumn: "PersonID",
                keyValue: new Guid("2a6d3738-9def-43ac-9279-0310edc7ceca"),
                column: "TIN",
                value: null);

            migrationBuilder.UpdateData(
                table: "People",
                keyColumn: "PersonID",
                keyValue: new Guid("89e5f445-d89f-4e12-94e0-5ad5b235d704"),
                column: "TIN",
                value: null);

            migrationBuilder.UpdateData(
                table: "People",
                keyColumn: "PersonID",
                keyValue: new Guid("a3b9833b-8a4d-43e9-8690-61e08df81a9a"),
                column: "TIN",
                value: null);

            migrationBuilder.UpdateData(
                table: "People",
                keyColumn: "PersonID",
                keyValue: new Guid("ac660a73-b0b7-4340-abc1-a914257a6189"),
                column: "TIN",
                value: null);

            migrationBuilder.UpdateData(
                table: "People",
                keyColumn: "PersonID",
                keyValue: new Guid("c03bbe45-9aeb-4d24-99e0-4743016ffce9"),
                column: "TIN",
                value: null);

            migrationBuilder.UpdateData(
                table: "People",
                keyColumn: "PersonID",
                keyValue: new Guid("c3abddbd-cf50-41d2-b6c4-cc7d5a750928"),
                column: "TIN",
                value: null);

            migrationBuilder.UpdateData(
                table: "People",
                keyColumn: "PersonID",
                keyValue: new Guid("c6d50a47-f7e6-4482-8be0-4ddfc057fa6e"),
                column: "TIN",
                value: null);

            migrationBuilder.UpdateData(
                table: "People",
                keyColumn: "PersonID",
                keyValue: new Guid("cb035f22-e7cf-4907-bd07-91cfee5240f3"),
                column: "TIN",
                value: null);

            migrationBuilder.UpdateData(
                table: "People",
                keyColumn: "PersonID",
                keyValue: new Guid("d15c6d9f-70b4-48c5-afd3-e71261f1a9be"),
                column: "TIN",
                value: null);
        }
    }
}
