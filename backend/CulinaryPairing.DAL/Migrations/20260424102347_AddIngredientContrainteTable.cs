using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CulinaryPairing.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddIngredientContrainteTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "INGREDIENT_CONTRAINTE",
                columns: table => new
                {
                    id_ingredient = table.Column<int>(type: "int", nullable: false),
                    id_contrainte = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_INGREDIENT_CONTRAINTE", x => new { x.id_ingredient, x.id_contrainte });
                    table.ForeignKey(
                        name: "FK_INGREDIENT_CONTRAINTE_CONTRAINTE_ALIMENTAIRE_id_contrainte",
                        column: x => x.id_contrainte,
                        principalTable: "CONTRAINTE_ALIMENTAIRE",
                        principalColumn: "id_contrainte",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_INGREDIENT_CONTRAINTE_INGREDIENT_id_ingredient",
                        column: x => x.id_ingredient,
                        principalTable: "INGREDIENT",
                        principalColumn: "id_ingredient",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_INGREDIENT_CONTRAINTE_id_contrainte",
                table: "INGREDIENT_CONTRAINTE",
                column: "id_contrainte");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "INGREDIENT_CONTRAINTE");
        }
    }
}
