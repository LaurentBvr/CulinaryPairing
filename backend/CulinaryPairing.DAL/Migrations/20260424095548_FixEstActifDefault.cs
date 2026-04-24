using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CulinaryPairing.DAL.Migrations
{
    /// <inheritdoc />
    public partial class FixEstActifDefault : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "est_actif",
                table: "UTILISATEUR",
                type: "bit",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "bit");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "est_actif",
                table: "UTILISATEUR",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: true);
        }
    }
}
