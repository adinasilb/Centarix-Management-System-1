using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class addedImageUrlToCategoryBase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ImageUrl",
                table: "ResourceCategories",
                newName: "ImageURL");

            migrationBuilder.AddColumn<string>(
                name: "ImageURL",
                table: "ParentCategories",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageURL",
                table: "ParentCategories");

            migrationBuilder.RenameColumn(
                name: "ImageURL",
                table: "ResourceCategories",
                newName: "ImageUrl");
        }
    }
}
