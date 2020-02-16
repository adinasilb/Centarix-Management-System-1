using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class OneToManyParentCategoryAndProductSubcategories : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductSubcategory_ParentCategory_ParentCategoryID",
                table: "ProductSubcategory");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductSubcategory",
                table: "ProductSubcategory");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ParentCategory",
                table: "ParentCategory");

            migrationBuilder.RenameTable(
                name: "ProductSubcategory",
                newName: "ProductSubcategories");

            migrationBuilder.RenameTable(
                name: "ParentCategory",
                newName: "ParentCategories");

            migrationBuilder.RenameIndex(
                name: "IX_ProductSubcategory_ParentCategoryID",
                table: "ProductSubcategories",
                newName: "IX_ProductSubcategories_ParentCategoryID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductSubcategories",
                table: "ProductSubcategories",
                column: "ProductSubcategoryID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ParentCategories",
                table: "ParentCategories",
                column: "ParentCategoryID");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductSubcategories_ParentCategories_ParentCategoryID",
                table: "ProductSubcategories",
                column: "ParentCategoryID",
                principalTable: "ParentCategories",
                principalColumn: "ParentCategoryID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductSubcategories_ParentCategories_ParentCategoryID",
                table: "ProductSubcategories");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductSubcategories",
                table: "ProductSubcategories");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ParentCategories",
                table: "ParentCategories");

            migrationBuilder.RenameTable(
                name: "ProductSubcategories",
                newName: "ProductSubcategory");

            migrationBuilder.RenameTable(
                name: "ParentCategories",
                newName: "ParentCategory");

            migrationBuilder.RenameIndex(
                name: "IX_ProductSubcategories_ParentCategoryID",
                table: "ProductSubcategory",
                newName: "IX_ProductSubcategory_ParentCategoryID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductSubcategory",
                table: "ProductSubcategory",
                column: "ProductSubcategoryID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ParentCategory",
                table: "ParentCategory",
                column: "ParentCategoryID");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductSubcategory_ParentCategory_ParentCategoryID",
                table: "ProductSubcategory",
                column: "ParentCategoryID",
                principalTable: "ParentCategory",
                principalColumn: "ParentCategoryID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
