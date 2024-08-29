using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BanqueTardiApp.Migrations
{
    /// <inheritdoc />
    public partial class AddTransactions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Transaction",
                columns: table => new
                {
                    Identifiant = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    TypeOperation = table.Column<int>(type: "INTEGER", nullable: false),
                    Montant = table.Column<decimal>(type: "TEXT", nullable: false),
                    DateOperation = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Libelle = table.Column<string>(type: "TEXT", maxLength: 250, nullable: false),
                    NumeroCompte = table.Column<string>(type: "TEXT", nullable: false),
                    CompteBancaireNumeroCompte = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transaction", x => x.Identifiant);
                    table.ForeignKey(
                        name: "FK_Transaction_ComptesBancaires_CompteBancaireNumeroCompte",
                        column: x => x.CompteBancaireNumeroCompte,
                        principalTable: "ComptesBancaires",
                        principalColumn: "NumeroCompte",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Transaction_CompteBancaireNumeroCompte",
                table: "Transaction",
                column: "CompteBancaireNumeroCompte");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Transaction");
        }
    }
}
