using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class FixedPsPics : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 1506);

            migrationBuilder.DeleteData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 1507);

            migrationBuilder.DeleteData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 1508);

            migrationBuilder.DeleteData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 1509);

            migrationBuilder.DeleteData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 1510);

            migrationBuilder.DeleteData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 1511);

            migrationBuilder.DeleteData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 1512);

            migrationBuilder.DeleteData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 1513);

            migrationBuilder.UpdateData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 204,
                column: "ProductSubcategoryDescription",
                value: "");

            migrationBuilder.UpdateData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 1501,
                column: "ImageURL",
                value: "/images/css/CategoryImages/consumables/general.png");

            migrationBuilder.UpdateData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 1502,
                column: "ImageURL",
                value: "/images/css/CategoryImages/reagents/general_reagents.png");

            migrationBuilder.UpdateData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 1503,
                column: "ImageURL",
                value: "/images/css/CategoryImages/biological/general.png");

            migrationBuilder.UpdateData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 1504,
                column: "ImageURL",
                value: "/images/css/CategoryImages/reusable/all_reusables.png");

            migrationBuilder.UpdateData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 1505,
                column: "ImageURL",
                value: "/images/css/CategoryImages/safety/safety.png");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 204,
                column: "ProductSubcategoryDescription",
                value: "Solution");

            migrationBuilder.UpdateData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 1501,
                column: "ImageURL",
                value: "/images/css/CategoryImages/defaultCategory.png");

            migrationBuilder.UpdateData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 1502,
                column: "ImageURL",
                value: "/images/css/CategoryImages/defaultCategory.png");

            migrationBuilder.UpdateData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 1503,
                column: "ImageURL",
                value: "/images/css/CategoryImages/defaultCategory.png");

            migrationBuilder.UpdateData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 1504,
                column: "ImageURL",
                value: "/images/css/CategoryImages/defaultCategory.png");

            migrationBuilder.UpdateData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 1505,
                column: "ImageURL",
                value: "/images/css/CategoryImages/defaultCategory.png");

            migrationBuilder.InsertData(
                table: "ProductSubcategories",
                columns: new[] { "ProductSubcategoryID", "ImageURL", "IsOldSubCategory", "ParentCategoryID", "ProductSubcategoryDescription" },
                values: new object[,]
                {
                    { 1506, "/images/css/CategoryImages/defaultCategory.png", true, 6, "Old Sub category" },
                    { 1507, "/images/css/CategoryImages/defaultCategory.png", true, 7, "Old Sub category" },
                    { 1508, "/images/css/CategoryImages/defaultCategory.png", true, 8, "Old Sub category" },
                    { 1509, "/images/css/CategoryImages/defaultCategory.png", true, 9, "Old Sub category" },
                    { 1510, "/images/css/CategoryImages/defaultCategory.png", true, 10, "Old Sub category" },
                    { 1511, "/images/css/CategoryImages/defaultCategory.png", true, 11, "Old Sub category" },
                    { 1512, "/images/css/CategoryImages/defaultCategory.png", true, 12, "Old Sub category" },
                    { 1513, "/images/css/CategoryImages/defaultCategory.png", true, 13, "Old Sub category" }
                });
        }
    }
}
