using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class addTipsToProductSubcategory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "ProductSubcategory",
                keyColumn: "ProductSubcategoryID",
                keyValue: 13,
                column: "ProductSubcategoryDescription",
                value: "Tips");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "ProductSubcategory",
                keyColumn: "ProductSubcategoryID",
                keyValue: 13,
                column: "ProductSubcategoryDescription",
                value: "Dishes");
        }
    }
}
