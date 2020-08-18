using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class AddOperationsLink : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Menus",
                keyColumn: "menuID",
                keyValue: 3,
                columns: new[] { "ActionName", "ControllerName" },
                values: new object[] { "Index", "Operations" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Menus",
                keyColumn: "menuID",
                keyValue: 3,
                columns: new[] { "ActionName", "ControllerName" },
                values: new object[] { "", "" });
        }
    }
}
