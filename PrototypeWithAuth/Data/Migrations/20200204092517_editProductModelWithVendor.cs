using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class editProductModelWithVendor : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Products_VendorID",
                table: "Products",
                column: "VendorID");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Vendors_VendorID",
                table: "Products",
                column: "VendorID",
                principalTable: "Vendors",
                principalColumn: "VendorID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Vendors_VendorID",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_VendorID",
                table: "Products");
        }
    }
}
