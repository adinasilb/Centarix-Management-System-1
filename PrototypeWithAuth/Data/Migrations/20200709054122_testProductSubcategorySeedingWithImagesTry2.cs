using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class testProductSubcategorySeedingWithImagesTry2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 102,
                column: "ImageURL",
                value: "/images/css/CategoryImages/PCR.png");

            migrationBuilder.UpdateData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 103,
                column: "ImageURL",
                value: "/images/css/CategoryImages/blood_tubes.png");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 102,
                column: "ImageURL",
                value: "~/images/css/CategoryImages/PCR.png");

            migrationBuilder.UpdateData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 103,
                column: "ImageURL",
                value: "~/images/css/CategoryImages/blood_tubes.png");
        }
    }
}
