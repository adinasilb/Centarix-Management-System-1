using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class RevertSamplesFieldToProprietary : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsSample",
                table: "ParentCategories");

            migrationBuilder.AddColumn<bool>(
                name: "IsProprietary",
                table: "ParentCategories",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "ParentCategories",
                keyColumn: "ParentCategoryID",
                keyValue: 7,
                column: "IsProprietary",
                value: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsProprietary",
                table: "ParentCategories");

            migrationBuilder.AddColumn<bool>(
                name: "IsSample",
                table: "ParentCategories",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "ParentCategories",
                keyColumn: "ParentCategoryID",
                keyValue: 7,
                column: "IsSample",
                value: true);
        }
    }
}
