using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class editProductModelWithProductSubcategory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Products_ProductSubcategoryID",
                table: "Products",
                column: "ProductSubcategoryID");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_ProductSubcategories_ProductSubcategoryID",
                table: "Products",
                column: "ProductSubcategoryID",
                principalTable: "ProductSubcategories",
                principalColumn: "ProductSubcategoryID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_ProductSubcategories_ProductSubcategoryID",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_ProductSubcategoryID",
                table: "Products");
        }
    }
}
