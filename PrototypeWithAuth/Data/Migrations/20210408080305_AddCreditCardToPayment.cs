using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class AddCreditCardToPayment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CreditCardID",
                table: "Payments",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Payments_CreditCardID",
                table: "Payments",
                column: "CreditCardID");

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_CreditCards_CreditCardID",
                table: "Payments",
                column: "CreditCardID",
                principalTable: "CreditCards",
                principalColumn: "CreditCardID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payments_CreditCards_CreditCardID",
                table: "Payments");

            migrationBuilder.DropIndex(
                name: "IX_Payments_CreditCardID",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "CreditCardID",
                table: "Payments");
        }
    }
}
