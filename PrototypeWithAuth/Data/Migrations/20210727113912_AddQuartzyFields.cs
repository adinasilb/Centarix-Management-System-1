using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class AddQuartzyFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsOldSubCategory",
                table: "ProductSubcategories",
                nullable: false,
                defaultValue: false);

            migrationBuilder.InsertData(
                table: "LocationTypes",
                columns: new[] { "LocationTypeID", "Depth", "Limit", "LocationTypeChildID", "LocationTypeName", "LocationTypeNameAbbre", "LocationTypeParentID", "LocationTypePluralName" },
                values: new object[] { 600, 0, 0, null, "Quartzy", "Q", null, "Quartzy" });

            migrationBuilder.InsertData(
                table: "ProductSubcategories",
                columns: new[] { "ProductSubcategoryID", "ImageURL", "IsOldSubCategory", "ParentCategoryID", "ProductSubcategoryDescription" },
                values: new object[,]
                {
                    { 1501, "/images/css/CategoryImages/defaultCategory.png", true, 1, "Old Subcategory" },
                    { 1502, "/images/css/CategoryImages/defaultCategory.png", true, 2, "Old Subcategory" },
                    { 1503, "/images/css/CategoryImages/defaultCategory.png", true, 3, "Old Subcategory" },
                    { 1504, "/images/css/CategoryImages/defaultCategory.png", true, 4, "Old Subcategory" },
                    { 1505, "/images/css/CategoryImages/defaultCategory.png", true, 5, "Old Subcategory" },
                    { 1506, "/images/css/CategoryImages/defaultCategory.png", true, 6, "Old Subcategory" },
                    { 1507, "/images/css/CategoryImages/defaultCategory.png", true, 7, "Old Subcategory" },
                    { 1508, "/images/css/CategoryImages/defaultCategory.png", true, 8, "Old Subcategory" },
                    { 1509, "/images/css/CategoryImages/defaultCategory.png", true, 9, "Old Subcategory" },
                    { 1510, "/images/css/CategoryImages/defaultCategory.png", true, 10, "Old Subcategory" },
                    { 1511, "/images/css/CategoryImages/defaultCategory.png", true, 11, "Old Subcategory" },
                    { 1512, "/images/css/CategoryImages/defaultCategory.png", true, 12, "Old Subcategory" },
                    { 1513, "/images/css/CategoryImages/defaultCategory.png", true, 13, "Old Subcategory" }
                });

            migrationBuilder.InsertData(
                table: "UnitTypes",
                columns: new[] { "UnitTypeID", "UnitParentTypeID", "UnitTypeDescription" },
                values: new object[] { -1, 1, "Quartzy Unit" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "LocationTypes",
                keyColumn: "LocationTypeID",
                keyValue: 600);

            migrationBuilder.DeleteData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 1501);

            migrationBuilder.DeleteData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 1502);

            migrationBuilder.DeleteData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 1503);

            migrationBuilder.DeleteData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 1504);

            migrationBuilder.DeleteData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 1505);

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

            migrationBuilder.DeleteData(
                table: "UnitTypes",
                keyColumn: "UnitTypeID",
                keyValue: -1);

            migrationBuilder.DropColumn(
                name: "IsOldSubCategory",
                table: "ProductSubcategories");
        }
    }
}
