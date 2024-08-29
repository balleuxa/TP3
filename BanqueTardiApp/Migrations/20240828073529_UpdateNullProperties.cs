using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BanqueTardiApp.Migrations
{
    /// <inheritdoc />
    public partial class UpdateNullProperties : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ComptesBancaires_Clients_ClientCode",
                table: "ComptesBancaires");

            migrationBuilder.DropForeignKey(
                name: "FK_Transaction_ComptesBancaires_CompteBancaireNumeroCompte",
                table: "Transaction");

            migrationBuilder.AlterColumn<string>(
                name: "CompteBancaireNumeroCompte",
                table: "Transaction",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<string>(
                name: "ClientCode",
                table: "ComptesBancaires",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AddForeignKey(
                name: "FK_ComptesBancaires_Clients_ClientCode",
                table: "ComptesBancaires",
                column: "ClientCode",
                principalTable: "Clients",
                principalColumn: "Code");

            migrationBuilder.AddForeignKey(
                name: "FK_Transaction_ComptesBancaires_CompteBancaireNumeroCompte",
                table: "Transaction",
                column: "CompteBancaireNumeroCompte",
                principalTable: "ComptesBancaires",
                principalColumn: "NumeroCompte");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ComptesBancaires_Clients_ClientCode",
                table: "ComptesBancaires");

            migrationBuilder.DropForeignKey(
                name: "FK_Transaction_ComptesBancaires_CompteBancaireNumeroCompte",
                table: "Transaction");

            migrationBuilder.AlterColumn<string>(
                name: "CompteBancaireNumeroCompte",
                table: "Transaction",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ClientCode",
                table: "ComptesBancaires",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ComptesBancaires_Clients_ClientCode",
                table: "ComptesBancaires",
                column: "ClientCode",
                principalTable: "Clients",
                principalColumn: "Code",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Transaction_ComptesBancaires_CompteBancaireNumeroCompte",
                table: "Transaction",
                column: "CompteBancaireNumeroCompte",
                principalTable: "ComptesBancaires",
                principalColumn: "NumeroCompte",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
