using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class UpdateProductSubcategories : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 103);

            migrationBuilder.DeleteData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 501);

            migrationBuilder.DeleteData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 502);

            migrationBuilder.DeleteData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 503);

            migrationBuilder.DeleteData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 504);

            migrationBuilder.DeleteData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 505);

            migrationBuilder.DeleteData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 506);

            migrationBuilder.DeleteData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 507);

            migrationBuilder.DeleteData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 508);

            migrationBuilder.DeleteData(
                table: "ParentCategories",
                keyColumn: "ParentCategoryID",
                keyValue: 5);

            migrationBuilder.UpdateData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 202,
                column: "ProductSubcategoryDescription",
                value: "Enzyme");

            migrationBuilder.UpdateData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 801,
                column: "ParentCategoryID",
                value: 8);

            migrationBuilder.UpdateData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 808,
                column: "ParentCategoryID",
                value: 8);

            migrationBuilder.UpdateData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 809,
                column: "ParentCategoryID",
                value: 8);

            migrationBuilder.InsertData(
                table: "ProductSubcategories",
                columns: new[] { "ProductSubcategoryID", "ImageURL", "ParentCategoryID", "ProductSubcategoryDescription" },
                values: new object[] { 109, null, 1, "Robot Tips" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 109);

            migrationBuilder.InsertData(
                table: "ParentCategories",
                columns: new[] { "ParentCategoryID", "CategoryTypeID", "ParentCategoryDescription", "isProprietary" },
                values: new object[] { 5, 1, "Equipment", false });

            migrationBuilder.UpdateData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 202,
                column: "ProductSubcategoryDescription",
                value: "DNA Enzyme");

            migrationBuilder.UpdateData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 801,
                column: "ParentCategoryID",
                value: 6);

            migrationBuilder.UpdateData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 808,
                column: "ParentCategoryID",
                value: 6);

            migrationBuilder.UpdateData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 809,
                column: "ParentCategoryID",
                value: 6);

            migrationBuilder.InsertData(
                table: "ProductSubcategories",
                columns: new[] { "ProductSubcategoryID", "ImageURL", "ParentCategoryID", "ProductSubcategoryDescription" },
                values: new object[] { 103, "/images/css/CategoryImages/blood_tubes.png", 1, "Blood Tubes" });

            migrationBuilder.InsertData(
                table: "ProductSubcategories",
                columns: new[] { "ProductSubcategoryID", "ImageURL", "ParentCategoryID", "ProductSubcategoryDescription" },
                values: new object[,]
                {
                    { 501, null, 5, "Instrument" },
                    { 502, null, 5, "Instrument Parts" },
                    { 503, null, 5, "Instrument Check" },
                    { 504, null, 5, "Instrument Fixing" },
                    { 505, null, 5, "Instrument Calibration" },
                    { 506, null, 5, "Instrument Warranty" },
                    { 507, null, 5, "Lab Software" },
                    { 508, null, 5, "Lab Furniture" }
                });
        }
    }
}
