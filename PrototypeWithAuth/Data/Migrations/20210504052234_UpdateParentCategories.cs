using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class UpdateParentCategories : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "ParentCategories",
                keyColumn: "ParentCategoryID",
                keyValue: 1,
                column: "ParentCategoryDescription",
                value: "Consumables");

            migrationBuilder.UpdateData(
                table: "ParentCategories",
                keyColumn: "ParentCategoryID",
                keyValue: 3,
                column: "ParentCategoryDescription",
                value: "Biological");

            migrationBuilder.UpdateData(
                table: "ParentCategories",
                keyColumn: "ParentCategoryID",
                keyValue: 6,
                columns: new[] { "CategoryTypeID", "ParentCategoryDescription" },
                values: new object[] { 1, "General" });

            migrationBuilder.UpdateData(
                table: "ParentCategories",
                keyColumn: "ParentCategoryID",
                keyValue: 7,
                columns: new[] { "IsProprietary", "ParentCategoryDescription" },
                values: new object[] { false, "Clinical" });

            migrationBuilder.UpdateData(
                table: "ParentCategories",
                keyColumn: "ParentCategoryID",
                keyValue: 8,
                column: "ParentCategoryDescription",
                value: "IT");

            migrationBuilder.UpdateData(
                table: "ParentCategories",
                keyColumn: "ParentCategoryID",
                keyValue: 9,
                column: "ParentCategoryDescription",
                value: "Day To Day");

            migrationBuilder.UpdateData(
                table: "ParentCategories",
                keyColumn: "ParentCategoryID",
                keyValue: 10,
                column: "ParentCategoryDescription",
                value: "Travel");

            migrationBuilder.UpdateData(
                table: "ParentCategories",
                keyColumn: "ParentCategoryID",
                keyValue: 11,
                column: "ParentCategoryDescription",
                value: "Advice");

            migrationBuilder.UpdateData(
                table: "ParentCategories",
                keyColumn: "ParentCategoryID",
                keyValue: 12,
                column: "ParentCategoryDescription",
                value: "Regulations");

            migrationBuilder.UpdateData(
                table: "ParentCategories",
                keyColumn: "ParentCategoryID",
                keyValue: 13,
                column: "ParentCategoryDescription",
                value: "Government");

            migrationBuilder.InsertData(
                table: "ParentCategories",
                columns: new[] { "ParentCategoryID", "CategoryTypeID", "IsProprietary", "ParentCategoryDescription" },
                values: new object[,]
                {
                    { 5, 1, false, "Safety" },
                    { 14, 1, true, "Samples" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ParentCategories",
                keyColumn: "ParentCategoryID",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "ParentCategories",
                keyColumn: "ParentCategoryID",
                keyValue: 14);

            migrationBuilder.UpdateData(
                table: "ParentCategories",
                keyColumn: "ParentCategoryID",
                keyValue: 1,
                column: "ParentCategoryDescription",
                value: "Plastics");

            migrationBuilder.UpdateData(
                table: "ParentCategories",
                keyColumn: "ParentCategoryID",
                keyValue: 3,
                column: "ParentCategoryDescription",
                value: "Cells");

            migrationBuilder.UpdateData(
                table: "ParentCategories",
                keyColumn: "ParentCategoryID",
                keyValue: 6,
                columns: new[] { "CategoryTypeID", "ParentCategoryDescription" },
                values: new object[] { 2, "IT" });

            migrationBuilder.UpdateData(
                table: "ParentCategories",
                keyColumn: "ParentCategoryID",
                keyValue: 7,
                columns: new[] { "IsProprietary", "ParentCategoryDescription" },
                values: new object[] { true, "Samples" });

            migrationBuilder.UpdateData(
                table: "ParentCategories",
                keyColumn: "ParentCategoryID",
                keyValue: 8,
                column: "ParentCategoryDescription",
                value: "Day To Day");

            migrationBuilder.UpdateData(
                table: "ParentCategories",
                keyColumn: "ParentCategoryID",
                keyValue: 9,
                column: "ParentCategoryDescription",
                value: "Travel");

            migrationBuilder.UpdateData(
                table: "ParentCategories",
                keyColumn: "ParentCategoryID",
                keyValue: 10,
                column: "ParentCategoryDescription",
                value: "Advisment");

            migrationBuilder.UpdateData(
                table: "ParentCategories",
                keyColumn: "ParentCategoryID",
                keyValue: 11,
                column: "ParentCategoryDescription",
                value: "Regulations");

            migrationBuilder.UpdateData(
                table: "ParentCategories",
                keyColumn: "ParentCategoryID",
                keyValue: 12,
                column: "ParentCategoryDescription",
                value: "Governments");

            migrationBuilder.UpdateData(
                table: "ParentCategories",
                keyColumn: "ParentCategoryID",
                keyValue: 13,
                column: "ParentCategoryDescription",
                value: "General");
        }
    }
}
