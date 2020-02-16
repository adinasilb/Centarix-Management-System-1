using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class addParentCategoryObjectToSubcategoryModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_ProductSubcategory_ParentCategoryID",
                table: "ProductSubcategory",
                column: "ParentCategoryID");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductSubcategory_ParentCategory_ParentCategoryID",
                table: "ProductSubcategory",
                column: "ParentCategoryID",
                principalTable: "ParentCategory",
                principalColumn: "ParentCategoryID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductSubcategory_ParentCategory_ParentCategoryID",
                table: "ProductSubcategory");

            migrationBuilder.DropIndex(
                name: "IX_ProductSubcategory_ParentCategoryID",
                table: "ProductSubcategory");
        }
    }
}
