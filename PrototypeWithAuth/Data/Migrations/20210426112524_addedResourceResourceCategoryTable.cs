using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class addedResourceResourceCategoryTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ResourceResourceCategory",
                columns: table => new
                {
                    ResourceID = table.Column<int>(nullable: false),
                    ResourceCategoryID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResourceResourceCategory", x => new { x.ResourceID, x.ResourceCategoryID });
                    table.ForeignKey(
                        name: "FK_ResourceResourceCategory_ResourceCategories_ResourceCategoryID",
                        column: x => x.ResourceCategoryID,
                        principalTable: "ResourceCategories",
                        principalColumn: "ResourceCategoryID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ResourceResourceCategory_Resources_ResourceID",
                        column: x => x.ResourceID,
                        principalTable: "Resources",
                        principalColumn: "ResourceID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ResourceResourceCategory_ResourceCategoryID",
                table: "ResourceResourceCategory",
                column: "ResourceCategoryID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ResourceResourceCategory");
        }
    }
}
