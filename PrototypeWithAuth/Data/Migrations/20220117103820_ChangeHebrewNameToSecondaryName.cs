using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class ChangeHebrewNameToSecondaryName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProductHebrewName",
                table: "Products");

            migrationBuilder.AddColumn<string>(
                name: "ProductSecondaryName",
                table: "Products",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProductSecondaryName",
                table: "Products");

            migrationBuilder.AddColumn<string>(
                name: "ProductHebrewName",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
