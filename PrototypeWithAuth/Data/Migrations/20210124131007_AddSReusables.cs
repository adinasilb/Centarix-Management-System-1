using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class AddSReusables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "ParentCategories",
                keyColumn: "ParentCategoryID",
                keyValue: 4,
                column: "ParentCategoryDescription",
                value: "Reusables");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "ParentCategories",
                keyColumn: "ParentCategoryID",
                keyValue: 4,
                column: "ParentCategoryDescription",
                value: "Reusable");
        }
    }
}
