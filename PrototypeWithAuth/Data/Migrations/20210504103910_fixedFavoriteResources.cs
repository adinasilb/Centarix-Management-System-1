using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class fixedFavoriteResources : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProtocolID",
                table: "FavoriteResources");

            migrationBuilder.AddColumn<int>(
                name: "ResourceID",
                table: "FavoriteResources",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ResourceID",
                table: "FavoriteResources");

            migrationBuilder.AddColumn<int>(
                name: "ProtocolID",
                table: "FavoriteResources",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
