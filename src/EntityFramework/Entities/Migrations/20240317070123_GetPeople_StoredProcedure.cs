using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Entities.Migrations
{
    /// <inheritdoc />
    public partial class GetPeople_StoredProcedure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            string sp_GetAllPeople = @"
                CREATE PROCEDURE [dbo].[GetAllPeople]
                AS BEGIN
                    SELECT PersonID, PersonName, Email, DateOfBirth, Gender, CountryID, Address, ReceiveNewsLetters FROM [dbo].[People]
                END
            ";
            migrationBuilder.Sql(sp_GetAllPeople);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            string sp_GetAllPeople = @"
                DROP PROCEDURE [dbo].[GetAllPeople]
                AS BEGIN
                    SELECT PersonID, PersonName, Email, DateOfBirth, Gender, CountryID, Address, ReceiveNewsLetters FROM [dbo].[People]
                END
            ";
            migrationBuilder.Sql(sp_GetAllPeople);
        }
    }
}
