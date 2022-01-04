using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class RenameFKProductSubcategoryParentCategory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_ProductSubcategories_ProductSubcategoryID",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductSubcategories_ParentCategories_ParentCategoryID",
                table: "ProductSubcategories");

            migrationBuilder.DropForeignKey(
                name: "FK_UnitTypeParentCategory_ParentCategories_ParentCategoryID",
                table: "UnitTypeParentCategory");


            migrationBuilder.RenameColumn(
                            name: "ProductSubcategoryID",
                            table: "ProductSubcategories",
                            newName: "ID");

            migrationBuilder.RenameColumn(
                            name: "ParentCategoryID",
                            table: "ParentCategories",
                            newName: "ID");


            migrationBuilder.RenameColumn(
                            name: "ProductSubcategoryDescription",
                            table: "ProductSubcategories",
                            newName: "Description");


            migrationBuilder.RenameColumn(
                            name: "ParentCategoryDescription",
                            table: "ParentCategories",
                            newName: "Description");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_ProductSubcategory",
                table: "Products",
                column: "ProductSubcategoryID",
                principalTable: "ProductSubcategories",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductSubcategories_ParentCategory",
                table: "ProductSubcategories",
                column: "ParentCategoryID",
                principalTable: "ParentCategories",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UnitTypeParentCategory_ParentCategories_ParentCategoryID",
                table: "UnitTypeParentCategory",
                column: "ParentCategoryID",
                principalTable: "ParentCategories",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_ProductSubcategory",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductSubcategories_ParentCategory",
                table: "ProductSubcategories");

            migrationBuilder.DropForeignKey(
                name: "FK_UnitTypeParentCategory_ParentCategories_ParentCategoryID",
                table: "UnitTypeParentCategory");


            migrationBuilder.RenameColumn(
                            name: "ID",
                            table: "ProductSubcategories",
                            newName: "ProductSubcategoryID");

            migrationBuilder.RenameColumn(
                            name: "ID",
                            table: "ParentCategories",
                            newName: "ParentCategoryID");


            migrationBuilder.RenameColumn(
                            name: "Description",
                            table: "ProductSubcategories",
                            newName: "ProductSubcategoryDescription");


            migrationBuilder.RenameColumn(
                            name: "Description",
                            table: "ParentCategories",
                            newName: "ParentCategoryDescription");


            migrationBuilder.AddForeignKey(
                name: "FK_Products_ProductSubcategories_ProductSubcategoryID",
                table: "Products",
                column: "ProductSubcategoryID",
                principalTable: "ProductSubcategories",
                principalColumn: "ProductSubcategoryID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductSubcategories_ParentCategories_ParentCategoryID",
                table: "ProductSubcategories",
                column: "ParentCategoryID",
                principalTable: "ParentCategories",
                principalColumn: "ParentCategoryID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UnitTypeParentCategory_ParentCategories_ParentCategoryID",
                table: "UnitTypeParentCategory",
                column: "ParentCategoryID",
                principalTable: "ParentCategories",
                principalColumn: "ParentCategoryID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
