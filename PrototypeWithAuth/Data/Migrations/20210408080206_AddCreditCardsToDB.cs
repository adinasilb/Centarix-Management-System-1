using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class AddCreditCardsToDB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "CreditCards",
                columns: new[] { "CreditCardID", "CardNumber", "CompanyAccountID" },
                values: new object[,]
                {
                    { 1, "2543", 2 },
                    { 2, "4694", 2 },
                    { 3, "3485", 2 },
                    { 4, "0054", 2 },
                    { 5, "4971", 1 },
                    { 6, "4424", 1 },
                    { 7, "4432", 1 },
                    { 8, "7972", 3 }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "CreditCards",
                keyColumn: "CreditCardID",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "CreditCards",
                keyColumn: "CreditCardID",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "CreditCards",
                keyColumn: "CreditCardID",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "CreditCards",
                keyColumn: "CreditCardID",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "CreditCards",
                keyColumn: "CreditCardID",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "CreditCards",
                keyColumn: "CreditCardID",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "CreditCards",
                keyColumn: "CreditCardID",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "CreditCards",
                keyColumn: "CreditCardID",
                keyValue: 8);
        }
    }
}
