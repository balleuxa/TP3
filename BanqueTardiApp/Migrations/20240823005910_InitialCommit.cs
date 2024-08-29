using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BanqueTardiApp.Migrations
{
    /// <inheritdoc />
    public partial class InitialCommit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Clients",
                columns: table => new
                {
                    Code = table.Column<string>(type: "TEXT", nullable: false),
                    Nom = table.Column<string>(type: "TEXT", maxLength: 150, nullable: false),
                    Prenom = table.Column<string>(type: "TEXT", maxLength: 150, nullable: false),
                    DateNaissance = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Adresse = table.Column<string>(type: "TEXT", maxLength: 250, nullable: false),
                    CodePostal = table.Column<string>(type: "TEXT", nullable: false),
                    Telephone = table.Column<string>(type: "TEXT", nullable: false),
                    NbDecouverts = table.Column<int>(type: "INTEGER", nullable: false),
                    NomParent = table.Column<string>(type: "TEXT", maxLength: 150, nullable: true),
                    TelephoneParent = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clients", x => x.Code);
                });

            migrationBuilder.CreateTable(
                name: "ComptesBancaires",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    NumeroCompte = table.Column<string>(type: "TEXT", nullable: false),
                    TypeCompte = table.Column<string>(type: "TEXT", nullable: false),
                    Solde = table.Column<decimal>(type: "TEXT", nullable: false),
                    ClientCode = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComptesBancaires", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ComptesBancaires_Clients_ClientCode",
                        column: x => x.ClientCode,
                        principalTable: "Clients",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Operations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Libelle = table.Column<string>(type: "TEXT", nullable: false),
                    Montant = table.Column<decimal>(type: "TEXT", nullable: false),
                    DateOperation = table.Column<DateTime>(type: "TEXT", nullable: false),
                    TypeOperation = table.Column<string>(type: "TEXT", nullable: false),
                    CompteBancaireId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Operations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Operations_ComptesBancaires_CompteBancaireId",
                        column: x => x.CompteBancaireId,
                        principalTable: "ComptesBancaires",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ComptesBancaires_ClientCode",
                table: "ComptesBancaires",
                column: "ClientCode");

            migrationBuilder.CreateIndex(
                name: "IX_Operations_CompteBancaireId",
                table: "Operations",
                column: "CompteBancaireId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Operations");

            migrationBuilder.DropTable(
                name: "ComptesBancaires");

            migrationBuilder.DropTable(
                name: "Clients");
        }
    }
}
