using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class MovedCatalogNumber : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CatalogNumber",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "IsApproved",
                table: "Requests");

            migrationBuilder.AddColumn<string>(
                name: "CatalogNumber",
                table: "Products",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CatalogNumber",
                table: "Products");

            migrationBuilder.AddColumn<string>(
                name: "CatalogNumber",
                table: "Requests",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsApproved",
                table: "Requests",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
