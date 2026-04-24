using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CulinaryPairing.DAL.Migrations
{
    /// <inheritdoc />
    public partial class ReplaceEstPublieeWithStatut : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // 1. Ajouter la colonne statut nullable (temporairement) avec default 'Brouillon'
            migrationBuilder.AddColumn<string>(
            name: "statut",
            table: "RECETTE",
            type: "nvarchar(20)",
            maxLength: 20,
            nullable: false,
            defaultValue: "Brouillon");

    // 2. Migrer les données : est_publiee=1 → 'Publiee'
            migrationBuilder.Sql(@"
            UPDATE RECETTE 
            SET statut = 'Publiee' 
            WHERE est_publiee = 1;
    ");

    // 3. Supprimer l'ancienne colonne
            migrationBuilder.DropColumn(
            name: "est_publiee",
            table: "RECETTE");
            }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "statut",
                table: "RECETTE");

            migrationBuilder.AddColumn<bool>(
                name: "est_publiee",
                table: "RECETTE",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
