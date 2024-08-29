using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BanqueTardiApp.Migrations
{
    /// <inheritdoc />
    public partial class changedIdCompte : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Operations_ComptesBancaires_CompteBancaireId",
                table: "Operations");

            migrationBuilder.DropIndex(
                name: "IX_Operations_CompteBancaireId",
                table: "Operations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ComptesBancaires",
                table: "ComptesBancaires");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "ComptesBancaires");

            migrationBuilder.AddColumn<string>(
                name: "CompteBancaireNumeroCompte",
                table: "Operations",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "NumeroCompte",
                table: "ComptesBancaires",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ComptesBancaires",
                table: "ComptesBancaires",
                column: "NumeroCompte");

            migrationBuilder.CreateIndex(
                name: "IX_Operations_CompteBancaireNumeroCompte",
                table: "Operations",
                column: "CompteBancaireNumeroCompte");

            migrationBuilder.AddForeignKey(
                name: "FK_Operations_ComptesBancaires_CompteBancaireNumeroCompte",
                table: "Operations",
                column: "CompteBancaireNumeroCompte",
                principalTable: "ComptesBancaires",
                principalColumn: "NumeroCompte",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Operations_ComptesBancaires_CompteBancaireNumeroCompte",
                table: "Operations");

            migrationBuilder.DropIndex(
                name: "IX_Operations_CompteBancaireNumeroCompte",
                table: "Operations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ComptesBancaires",
                table: "ComptesBancaires");

            migrationBuilder.DropColumn(
                name: "CompteBancaireNumeroCompte",
                table: "Operations");

            migrationBuilder.AlterColumn<string>(
                name: "NumeroCompte",
                table: "ComptesBancaires",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "ComptesBancaires",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0)
                .Annotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ComptesBancaires",
                table: "ComptesBancaires",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Operations_CompteBancaireId",
                table: "Operations",
                column: "CompteBancaireId");

            migrationBuilder.AddForeignKey(
                name: "FK_Operations_ComptesBancaires_CompteBancaireId",
                table: "Operations",
                column: "CompteBancaireId",
                principalTable: "ComptesBancaires",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
