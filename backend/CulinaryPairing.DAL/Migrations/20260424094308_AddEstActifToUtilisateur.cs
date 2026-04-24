using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CulinaryPairing.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddEstActifToUtilisateur : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "est_actif",
                table: "UTILISATEUR",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "est_actif",
                table: "UTILISATEUR");
        }
    }
}
