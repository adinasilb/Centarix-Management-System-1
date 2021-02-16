using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class FixProprietaryName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "ParentCategories",
                keyColumn: "ParentCategoryID",
                keyValue: 7,
                column: "ParentCategoryDescription",
                value: "Proprietary");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "ParentCategories",
                keyColumn: "ParentCategoryID",
                keyValue: 7,
                column: "ParentCategoryDescription",
                value: "Proprietry");
        }
    }
}
