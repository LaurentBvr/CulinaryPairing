using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CulinaryPairing.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddOriginFieldsToBoisson : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "appellation",
                table: "BOISSON",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "cepage",
                table: "BOISSON",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "pays",
                table: "BOISSON",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "region",
                table: "BOISSON",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "appellation",
                table: "BOISSON");

            migrationBuilder.DropColumn(
                name: "cepage",
                table: "BOISSON");

            migrationBuilder.DropColumn(
                name: "pays",
                table: "BOISSON");

            migrationBuilder.DropColumn(
                name: "region",
                table: "BOISSON");
        }
    }
}
