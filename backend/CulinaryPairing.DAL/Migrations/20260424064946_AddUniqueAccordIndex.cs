using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CulinaryPairing.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddUniqueAccordIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ACCORD_id_recette",
                table: "ACCORD");

            migrationBuilder.CreateIndex(
                name: "IX_ACCORD_id_recette_id_boisson_type_accord",
                table: "ACCORD",
                columns: new[] { "id_recette", "id_boisson", "type_accord" },
                unique: true,
                filter: "[type_accord] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ACCORD_id_recette_id_boisson_type_accord",
                table: "ACCORD");

            migrationBuilder.CreateIndex(
                name: "IX_ACCORD_id_recette",
                table: "ACCORD",
                column: "id_recette");
        }
    }
}
