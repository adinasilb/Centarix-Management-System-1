using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class updatedPaymentStatus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "PaymentStatuses",
                keyColumn: "PaymentStatusID",
                keyValue: 1,
                column: "PaymentStatusDescription",
                value: "No Invoice");

            migrationBuilder.UpdateData(
                table: "PaymentStatuses",
                keyColumn: "PaymentStatusID",
                keyValue: 2,
                column: "PaymentStatusDescription",
                value: "Not Paid");

            migrationBuilder.UpdateData(
                table: "PaymentStatuses",
                keyColumn: "PaymentStatusID",
                keyValue: 3,
                column: "PaymentStatusDescription",
                value: "Pay Now");

            migrationBuilder.UpdateData(
                table: "PaymentStatuses",
                keyColumn: "PaymentStatusID",
                keyValue: 4,
                column: "PaymentStatusDescription",
                value: "Pay Later");

            migrationBuilder.InsertData(
                table: "PaymentStatuses",
                columns: new[] { "PaymentStatusID", "PaymentStatusDescription" },
                values: new object[,]
                {
                    { 5, "Installments" },
                    { 6, "Paid" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "PaymentStatuses",
                keyColumn: "PaymentStatusID",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "PaymentStatuses",
                keyColumn: "PaymentStatusID",
                keyValue: 6);

            migrationBuilder.UpdateData(
                table: "PaymentStatuses",
                keyColumn: "PaymentStatusID",
                keyValue: 1,
                column: "PaymentStatusDescription",
                value: "None");

            migrationBuilder.UpdateData(
                table: "PaymentStatuses",
                keyColumn: "PaymentStatusID",
                keyValue: 2,
                column: "PaymentStatusDescription",
                value: "Pay Now");

            migrationBuilder.UpdateData(
                table: "PaymentStatuses",
                keyColumn: "PaymentStatusID",
                keyValue: 3,
                column: "PaymentStatusDescription",
                value: "Installments");

            migrationBuilder.UpdateData(
                table: "PaymentStatuses",
                keyColumn: "PaymentStatusID",
                keyValue: 4,
                column: "PaymentStatusDescription",
                value: "Paid");
        }
    }
}
