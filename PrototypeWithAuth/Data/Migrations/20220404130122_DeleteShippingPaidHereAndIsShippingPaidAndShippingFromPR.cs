using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class DeleteShippingPaidHereAndIsShippingPaidAndShippingFromPR : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ShippingPaidHere",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "IsShippingPaid",
                table: "ParentRequests");

            migrationBuilder.DropColumn(
                name: "Shipping",
                table: "ParentRequests");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "ShippingPaidHere",
                table: "Payments",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsShippingPaid",
                table: "ParentRequests",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<double>(
                name: "Shipping",
                table: "ParentRequests",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}
