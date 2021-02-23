using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class AddNewRequestFieldsMigrationDataForPayments : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "PaymentStatuses",
                keyColumn: "PaymentStatusID",
                keyValue: 1);

            migrationBuilder.AddColumn<bool>(
                name: "HasInvoice",
                table: "Requests",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsClarify",
                table: "Requests",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsPartial",
                table: "Requests",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "PaymentStatuses",
                keyColumn: "PaymentStatusID",
                keyValue: 2,
                column: "PaymentStatusDescription",
                value: "+ 30");

            migrationBuilder.UpdateData(
                table: "PaymentStatuses",
                keyColumn: "PaymentStatusID",
                keyValue: 4,
                column: "PaymentStatusDescription",
                value: "Pay Upon Arrival");

            migrationBuilder.InsertData(
                table: "PaymentStatuses",
                columns: new[] { "PaymentStatusID", "PaymentStatusDescription" },
                values: new object[,]
                {
                    { 7, "Standing Order" },
                    { 8, "Specify Payment Later" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "PaymentStatuses",
                keyColumn: "PaymentStatusID",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "PaymentStatuses",
                keyColumn: "PaymentStatusID",
                keyValue: 8);

            migrationBuilder.DropColumn(
                name: "HasInvoice",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "IsClarify",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "IsPartial",
                table: "Requests");

            migrationBuilder.UpdateData(
                table: "PaymentStatuses",
                keyColumn: "PaymentStatusID",
                keyValue: 2,
                column: "PaymentStatusDescription",
                value: "Not Paid");

            migrationBuilder.UpdateData(
                table: "PaymentStatuses",
                keyColumn: "PaymentStatusID",
                keyValue: 4,
                column: "PaymentStatusDescription",
                value: "Pay Later");

            migrationBuilder.InsertData(
                table: "PaymentStatuses",
                columns: new[] { "PaymentStatusID", "PaymentStatusDescription" },
                values: new object[] { 1, "No Invoice" });
        }
    }
}
