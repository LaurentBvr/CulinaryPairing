using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CulinaryPairing.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddAuditFieldsAndEngineVersion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "date_modification",
                table: "RECETTE",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "date_modification",
                table: "BOISSON",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "date_calcul",
                table: "ACCORD",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETUTCDATE()");

            migrationBuilder.AddColumn<string>(
                name: "version_moteur",
                table: "ACCORD",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "1.2");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "date_modification",
                table: "RECETTE");

            migrationBuilder.DropColumn(
                name: "date_modification",
                table: "BOISSON");

            migrationBuilder.DropColumn(
                name: "date_calcul",
                table: "ACCORD");

            migrationBuilder.DropColumn(
                name: "version_moteur",
                table: "ACCORD");
        }
    }
}
