using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class ChangeProprietaryToSamples : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "isProprietary",
                table: "ParentCategories");

            migrationBuilder.AddColumn<bool>(
                name: "IsSample",
                table: "ParentCategories",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "ParentCategories",
                keyColumn: "ParentCategoryID",
                keyValue: 7,
                column: "IsSample",
                value: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsSample",
                table: "ParentCategories");

            migrationBuilder.AddColumn<bool>(
                name: "isProprietary",
                table: "ParentCategories",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "ParentCategories",
                keyColumn: "ParentCategoryID",
                keyValue: 7,
                column: "isProprietary",
                value: true);
        }
    }
}
