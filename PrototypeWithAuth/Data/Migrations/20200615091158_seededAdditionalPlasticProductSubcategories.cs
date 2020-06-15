using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class seededAdditionalPlasticProductSubcategories : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "ProductSubcategories",
                columns: new[] { "ProductSubcategoryID", "ParentCategoryID", "ProductSubcategoryDescription" },
                values: new object[] { 16, 1, "Blood" });

            migrationBuilder.InsertData(
                table: "ProductSubcategories",
                columns: new[] { "ProductSubcategoryID", "ParentCategoryID", "ProductSubcategoryDescription" },
                values: new object[] { 17, 1, "3D Cells Grow" });

            migrationBuilder.InsertData(
                table: "ProductSubcategories",
                columns: new[] { "ProductSubcategoryID", "ParentCategoryID", "ProductSubcategoryDescription" },
                values: new object[] { 18, 1, "PCR Plates" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 18);
        }
    }
}
