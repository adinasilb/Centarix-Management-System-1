using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class createdCategoryTypeAndCategoryTypeParentCategoryTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CategoryTypes",
                columns: table => new
                {
                    CategoryTypeID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoryTypeDescription = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoryTypes", x => x.CategoryTypeID);
                });

            migrationBuilder.CreateTable(
                name: "CategoryTypeParentCategory",
                columns: table => new
                {
                    CategoryTypeID = table.Column<int>(nullable: false),
                    ParentCategoryID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoryTypeParentCategory", x => new { x.CategoryTypeID, x.ParentCategoryID });
                    table.ForeignKey(
                        name: "FK_CategoryTypeParentCategory_CategoryTypes_CategoryTypeID",
                        column: x => x.CategoryTypeID,
                        principalTable: "CategoryTypes",
                        principalColumn: "CategoryTypeID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CategoryTypeParentCategory_ParentCategories_ParentCategoryID",
                        column: x => x.ParentCategoryID,
                        principalTable: "ParentCategories",
                        principalColumn: "ParentCategoryID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CategoryTypeParentCategory_ParentCategoryID",
                table: "CategoryTypeParentCategory",
                column: "ParentCategoryID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CategoryTypeParentCategory");

            migrationBuilder.DropTable(
                name: "CategoryTypes");
        }
    }
}
