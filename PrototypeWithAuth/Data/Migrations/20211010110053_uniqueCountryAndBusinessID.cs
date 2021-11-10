using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class uniqueCountryAndBusinessID : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "VendorBuisnessID",
                table: "Vendors",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_Vendors_VendorCountry_VendorBuisnessID",
                table: "Vendors",
                columns: new[] { "VendorCountry", "VendorBuisnessID" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Vendors_VendorCountry_VendorBuisnessID",
                table: "Vendors");

            migrationBuilder.AlterColumn<string>(
                name: "VendorBuisnessID",
                table: "Vendors",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string));
        }
    }
}
