using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CulinaryPairing.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddAccordBoissonScoreIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ACCORD_id_boisson",
                table: "ACCORD");

            migrationBuilder.CreateIndex(
                name: "IX_Accord_Boisson_Score",
                table: "ACCORD",
                columns: new[] { "id_boisson", "score_compatibilite" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Accord_Boisson_Score",
                table: "ACCORD");

            migrationBuilder.CreateIndex(
                name: "IX_ACCORD_id_boisson",
                table: "ACCORD",
                column: "id_boisson");
        }
    }
}
