using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class InventorySubcategoriesInDBContext : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Inventories_InventorySubcategory_InventorySubcategoryID",
                table: "Inventories");

            migrationBuilder.DropPrimaryKey(
                name: "PK_InventorySubcategory",
                table: "InventorySubcategory");

            migrationBuilder.RenameTable(
                name: "InventorySubcategory",
                newName: "InventorySubcategories");

            migrationBuilder.AddPrimaryKey(
                name: "PK_InventorySubcategories",
                table: "InventorySubcategories",
                column: "InventorySubcategoryID");

            migrationBuilder.AddForeignKey(
                name: "FK_Inventories_InventorySubcategories_InventorySubcategoryID",
                table: "Inventories",
                column: "InventorySubcategoryID",
                principalTable: "InventorySubcategories",
                principalColumn: "InventorySubcategoryID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Inventories_InventorySubcategories_InventorySubcategoryID",
                table: "Inventories");

            migrationBuilder.DropPrimaryKey(
                name: "PK_InventorySubcategories",
                table: "InventorySubcategories");

            migrationBuilder.RenameTable(
                name: "InventorySubcategories",
                newName: "InventorySubcategory");

            migrationBuilder.AddPrimaryKey(
                name: "PK_InventorySubcategory",
                table: "InventorySubcategory",
                column: "InventorySubcategoryID");

            migrationBuilder.AddForeignKey(
                name: "FK_Inventories_InventorySubcategory_InventorySubcategoryID",
                table: "Inventories",
                column: "InventorySubcategoryID",
                principalTable: "InventorySubcategory",
                principalColumn: "InventorySubcategoryID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
