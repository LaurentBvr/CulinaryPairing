using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CulinaryPairing.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddMissingCheckConstraints : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
        // 1. SOIREE : nombre végétariens + végans <= nombre personnes
        migrationBuilder.Sql(@"
        ALTER TABLE SOIREE 
        ADD CONSTRAINT CK_Soiree_NombreVegeCoherent 
        CHECK (nombre_vegetariens + nombre_vegans <= nombre_personnes);
    ");

        // 2. SUBSTITUTION_INGREDIENT : pas de substitution sur soi-même
        migrationBuilder.Sql(@"
        ALTER TABLE SUBSTITUTION_INGREDIENT 
        ADD CONSTRAINT CK_Substitution_IngredientsDifferents 
        CHECK (id_ingredient_original <> id_ingredient_substitut);
    ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
        migrationBuilder.Sql("ALTER TABLE SUBSTITUTION_INGREDIENT DROP CONSTRAINT CK_Substitution_IngredientsDifferents;");
        migrationBuilder.Sql("ALTER TABLE SOIREE DROP CONSTRAINT CK_Soiree_NombreVegeCoherent;");
        }
    }
}
