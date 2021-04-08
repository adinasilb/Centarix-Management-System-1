using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class UpdatePaymentTypes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "PaymentTypes",
                keyColumn: "PaymentTypeID",
                keyValue: 2,
                column: "PaymentTypeDescription",
                value: "Check");

            migrationBuilder.InsertData(
                table: "PaymentTypes",
                columns: new[] { "PaymentTypeID", "PaymentTypeDescription" },
                values: new object[] { 3, "Wire" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "PaymentTypes",
                keyColumn: "PaymentTypeID",
                keyValue: 3);

            migrationBuilder.UpdateData(
                table: "PaymentTypes",
                keyColumn: "PaymentTypeID",
                keyValue: 2,
                column: "PaymentTypeDescription",
                value: "Bank Transfer");
        }
    }
}
