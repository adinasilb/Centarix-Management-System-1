using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class removedVendorCountrySwitchedToIDinDBContext : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Vendors_CountryID",
                table: "Vendors");

            migrationBuilder.DropIndex(
                name: "IX_Vendors_VendorCountry_VendorBuisnessID",
                table: "Vendors");

            migrationBuilder.DropColumn(
                name: "VendorCountry",
                table: "Vendors");

            migrationBuilder.CreateIndex(
                name: "IX_Vendors_CountryID_VendorBuisnessID",
                table: "Vendors",
                columns: new[] { "CountryID", "VendorBuisnessID" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Vendors_CountryID_VendorBuisnessID",
                table: "Vendors");

            migrationBuilder.AddColumn<string>(
                name: "VendorCountry",
                table: "Vendors",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Vendors_CountryID",
                table: "Vendors",
                column: "CountryID");

            migrationBuilder.CreateIndex(
                name: "IX_Vendors_VendorCountry_VendorBuisnessID",
                table: "Vendors",
                columns: new[] { "VendorCountry", "VendorBuisnessID" },
                unique: true);
        }
    }
}
