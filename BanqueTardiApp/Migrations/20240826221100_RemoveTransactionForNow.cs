using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BanqueTardiApp.Migrations
{
    /// <inheritdoc />
    public partial class RemoveTransactionForNow : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Operations");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Operations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CompteBancaireNumeroCompte = table.Column<string>(type: "TEXT", nullable: false),
                    CompteBancaireId = table.Column<int>(type: "INTEGER", nullable: false),
                    DateOperation = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Libelle = table.Column<string>(type: "TEXT", nullable: false),
                    Montant = table.Column<decimal>(type: "TEXT", nullable: false),
                    TypeOperation = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Operations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Operations_ComptesBancaires_CompteBancaireNumeroCompte",
                        column: x => x.CompteBancaireNumeroCompte,
                        principalTable: "ComptesBancaires",
                        principalColumn: "NumeroCompte",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Operations_CompteBancaireNumeroCompte",
                table: "Operations",
                column: "CompteBancaireNumeroCompte");
        }
    }
}
