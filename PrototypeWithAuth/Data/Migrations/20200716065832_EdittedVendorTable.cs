using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class EdittedVendorTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ContactEmail",
                table: "Vendors");

            migrationBuilder.DropColumn(
                name: "ContactPerson",
                table: "Vendors");

            migrationBuilder.DropColumn(
                name: "OrderEmail",
                table: "Vendors");

            migrationBuilder.DropColumn(
                name: "VendorContactPhone1",
                table: "Vendors");

            migrationBuilder.DropColumn(
                name: "VendorContactPhone2",
                table: "Vendors");

            migrationBuilder.AddColumn<string>(
                name: "InfoEmail",
                table: "Vendors",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OrdersEmail",
                table: "Vendors",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VendorCellPhone",
                table: "Vendors",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VendorCountry",
                table: "Vendors",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VendorTelephone",
                table: "Vendors",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InfoEmail",
                table: "Vendors");

            migrationBuilder.DropColumn(
                name: "OrdersEmail",
                table: "Vendors");

            migrationBuilder.DropColumn(
                name: "VendorCellPhone",
                table: "Vendors");

            migrationBuilder.DropColumn(
                name: "VendorCountry",
                table: "Vendors");

            migrationBuilder.DropColumn(
                name: "VendorTelephone",
                table: "Vendors");

            migrationBuilder.AddColumn<string>(
                name: "ContactEmail",
                table: "Vendors",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ContactPerson",
                table: "Vendors",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OrderEmail",
                table: "Vendors",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VendorContactPhone1",
                table: "Vendors",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VendorContactPhone2",
                table: "Vendors",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
