using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CulinaryPairing.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddPerformanceIndexes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_HISTORIQUE_CONSULTATION_id_utilisateur",
                table: "HISTORIQUE_CONSULTATION");

            migrationBuilder.CreateIndex(
                name: "IX_Recette_Statut",
                table: "RECETTE",
                column: "statut");

            migrationBuilder.CreateIndex(
                name: "IX_Recette_TypePlat",
                table: "RECETTE",
                column: "type_plat");

            migrationBuilder.CreateIndex(
                name: "IX_Historique_Utilisateur_Recette_Date",
                table: "HISTORIQUE_CONSULTATION",
                columns: new[] { "id_utilisateur", "id_recette", "date_consultation" },
                descending: new[] { false, false, true });

            migrationBuilder.CreateIndex(
                name: "IX_Boisson_Type",
                table: "BOISSON",
                column: "type_boisson");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Recette_Statut",
                table: "RECETTE");

            migrationBuilder.DropIndex(
                name: "IX_Recette_TypePlat",
                table: "RECETTE");

            migrationBuilder.DropIndex(
                name: "IX_Historique_Utilisateur_Recette_Date",
                table: "HISTORIQUE_CONSULTATION");

            migrationBuilder.DropIndex(
                name: "IX_Boisson_Type",
                table: "BOISSON");

            migrationBuilder.CreateIndex(
                name: "IX_HISTORIQUE_CONSULTATION_id_utilisateur",
                table: "HISTORIQUE_CONSULTATION",
                column: "id_utilisateur");
        }
    }
}
