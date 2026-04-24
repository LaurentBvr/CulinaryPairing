using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CulinaryPairing.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddFavoriMenuTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FAVORI_MENU",
                columns: table => new
                {
                    id_utilisateur = table.Column<int>(type: "int", nullable: false),
                    id_menu = table.Column<int>(type: "int", nullable: false),
                    date_ajout = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FAVORI_MENU", x => new { x.id_utilisateur, x.id_menu });
                    table.ForeignKey(
                        name: "FK_FAVORI_MENU_MENU_SOIREE_id_menu",
                        column: x => x.id_menu,
                        principalTable: "MENU_SOIREE",
                        principalColumn: "id_menu",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FAVORI_MENU_UTILISATEUR_id_utilisateur",
                        column: x => x.id_utilisateur,
                        principalTable: "UTILISATEUR",
                        principalColumn: "id_utilisateur",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FAVORI_MENU_id_menu",
                table: "FAVORI_MENU",
                column: "id_menu");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FAVORI_MENU");
        }
    }
}
