using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class UnitParentCategoryManytoMany : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UnitTypeParentCategory",
                columns: table => new
                {
                    UnitTypeID = table.Column<int>(nullable: false),
                    ParentCategoryID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UnitTypeParentCategory", x => new { x.UnitTypeID, x.ParentCategoryID });
                    table.ForeignKey(
                        name: "FK_UnitTypeParentCategory_ParentCategories_ParentCategoryID",
                        column: x => x.ParentCategoryID,
                        principalTable: "ParentCategories",
                        principalColumn: "ParentCategoryID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UnitTypeParentCategory_UnitTypes_UnitTypeID",
                        column: x => x.UnitTypeID,
                        principalTable: "UnitTypes",
                        principalColumn: "UnitTypeID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UnitTypeParentCategory_ParentCategoryID",
                table: "UnitTypeParentCategory",
                column: "ParentCategoryID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UnitTypeParentCategory");
        }
    }
}
