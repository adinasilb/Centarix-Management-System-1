using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class seededPaymentStatuses : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "PaymentStatuses",
                columns: new[] { "PaymentStatusID", "PaymentStatusDescription" },
                values: new object[,]
                {
                    { 1, "None" },
                    { 2, "Pay Now" },
                    { 3, "Installments" },
                    { 4, "Paid" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "PaymentStatuses",
                keyColumn: "PaymentStatusID",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "PaymentStatuses",
                keyColumn: "PaymentStatusID",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "PaymentStatuses",
                keyColumn: "PaymentStatusID",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "PaymentStatuses",
                keyColumn: "PaymentStatusID",
                keyValue: 4);
        }
    }
}
