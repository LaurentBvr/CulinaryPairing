using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CulinaryPairing.DAL.Migrations
{
    /// <inheritdoc />
    /// <summary>
    /// Aligne les valeurs de l'enum TypeContrainte avec le vocabulaire du CdC §3.5.1.
    /// Mapping : Religieux→Conviction ; Sante→Allergie (Allergie arachides, Allergie fruits de mer)
    /// + Regime (Sans lactose, Sans gluten) ; Choix→Regime (Végétarien, Végan).
    /// Migration data-only : pas de changement de schéma, la colonne reste nvarchar(20).
    /// </summary>
    public partial class AlignTypeContrainteWithCdC : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // 1. Religieux → Conviction (Halal, Casher)
            migrationBuilder.Sql(
                "UPDATE CONTRAINTE_ALIMENTAIRE SET type = N'Conviction' WHERE type = N'Religieux';");

            // 2. Sante → Allergie (uniquement les vraies allergies, par Nom).
            //    DOIT précéder l'étape 3, sinon les allergies seraient absorbées par le Sante→Regime.
            migrationBuilder.Sql(
                "UPDATE CONTRAINTE_ALIMENTAIRE SET type = N'Allergie' " +
                "WHERE nom IN (N'Allergie arachides', N'Allergie fruits de mer');");

            // 3. Sante restant + Choix → Regime
            //    Sante restant = Sans lactose, Sans gluten (intolérances/évictions médicales)
            //    Choix = Végétarien, Végan (régimes alimentaires)
            migrationBuilder.Sql(
                "UPDATE CONTRAINTE_ALIMENTAIRE SET type = N'Regime' " +
                "WHERE type IN (N'Choix', N'Sante');");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Rollback exact : retour au mapping initial (Religieux / Sante / Choix).
            migrationBuilder.Sql(
                "UPDATE CONTRAINTE_ALIMENTAIRE SET type = N'Religieux' WHERE type = N'Conviction';");

            migrationBuilder.Sql(
                "UPDATE CONTRAINTE_ALIMENTAIRE SET type = N'Sante' " +
                "WHERE nom IN (N'Allergie arachides', N'Allergie fruits de mer', " +
                "N'Sans lactose', N'Sans gluten');");

            migrationBuilder.Sql(
                "UPDATE CONTRAINTE_ALIMENTAIRE SET type = N'Choix' " +
                "WHERE nom IN (N'Végétarien', N'Végan');");
        }
    }
}