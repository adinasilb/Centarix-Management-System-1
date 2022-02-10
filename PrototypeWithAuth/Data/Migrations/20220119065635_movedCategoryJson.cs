using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class movedCategoryJson : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CategoryJsons");

            migrationBuilder.AddColumn<string>(
                name: "CategoryJson",
                table: "ResourceCategories",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CategoryJsonID",
                table: "ResourceCategories",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "CategoryJson",
                table: "ProductSubcategories",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CategoryJsonID",
                table: "ProductSubcategories",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "CategoryJson",
                table: "ParentCategories",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CategoryJsonID",
                table: "ParentCategories",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CategoryJson",
                table: "ResourceCategories");

            migrationBuilder.DropColumn(
                name: "CategoryJsonID",
                table: "ResourceCategories");

            migrationBuilder.DropColumn(
                name: "CategoryJson",
                table: "ProductSubcategories");

            migrationBuilder.DropColumn(
                name: "CategoryJsonID",
                table: "ProductSubcategories");

            migrationBuilder.DropColumn(
                name: "CategoryJson",
                table: "ParentCategories");

            migrationBuilder.DropColumn(
                name: "CategoryJsonID",
                table: "ParentCategories");

            migrationBuilder.CreateTable(
                name: "CategoryJsons",
                columns: table => new
                {
                    CategoryJsonID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoryBaseID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoryJsons", x => x.CategoryJsonID);
                });
        }
    }
}
