using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class AddInventorySubcategory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "InventorySubcategoryID",
                table: "Inventories",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "InventorySubcategory",
                columns: table => new
                {
                    InventorySubcategoryID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InventorySubcategoryDescription = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InventorySubcategory", x => x.InventorySubcategoryID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Inventories_InventorySubcategoryID",
                table: "Inventories",
                column: "InventorySubcategoryID");

            migrationBuilder.AddForeignKey(
                name: "FK_Inventories_InventorySubcategory_InventorySubcategoryID",
                table: "Inventories",
                column: "InventorySubcategoryID",
                principalTable: "InventorySubcategory",
                principalColumn: "InventorySubcategoryID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Inventories_InventorySubcategory_InventorySubcategoryID",
                table: "Inventories");

            migrationBuilder.DropTable(
                name: "InventorySubcategory");

            migrationBuilder.DropIndex(
                name: "IX_Inventories_InventorySubcategoryID",
                table: "Inventories");

            migrationBuilder.DropColumn(
                name: "InventorySubcategoryID",
                table: "Inventories");
        }
    }
}
