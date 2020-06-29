using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class seededAdditinalParentCatgorie : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "ParentCategories",
                columns: new[] { "ParentCategoryID", "ParentCategoryDescription" },
                values: new object[] { 5, "Equipment" });

            migrationBuilder.InsertData(
                table: "ParentCategories",
                columns: new[] { "ParentCategoryID", "ParentCategoryDescription" },
                values: new object[] { 6, "Operation" });
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
                keyValue: 6);
        }
    }
}
