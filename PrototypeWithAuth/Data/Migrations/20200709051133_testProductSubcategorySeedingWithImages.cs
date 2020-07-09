using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class testProductSubcategorySeedingWithImages : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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
                columns: new[] { "ImageURL", "ProductSubcategoryDescription" },
                values: new object[] { "~/images/css/CategoryImages/blood_tubes.png", "Blood Tubes" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 102,
                column: "ImageURL",
                value: null);

            migrationBuilder.UpdateData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 103,
                columns: new[] { "ImageURL", "ProductSubcategoryDescription" },
                values: new object[] { null, "Blood" });
        }
    }
}
