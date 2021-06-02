using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class HasInvoiceAndIsPaidOnPayment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HasInvoice",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "Paid",
                table: "Requests");

            migrationBuilder.AddColumn<bool>(
                name: "HasInvoice",
                table: "Payments",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HasInvoice",
                table: "Payments");

            migrationBuilder.AddColumn<bool>(
                name: "HasInvoice",
                table: "Requests",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Paid",
                table: "Requests",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
