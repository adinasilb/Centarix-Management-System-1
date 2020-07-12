using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class seededMenuViewName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Menus",
                keyColumn: "menuID",
                keyValue: 1,
                columns: new[] { "MenuDescription", "MenuViewName" },
                values: new object[] { "OrdersAndInventory", "Orders & Inventory" });

            migrationBuilder.UpdateData(
                table: "Menus",
                keyColumn: "menuID",
                keyValue: 2,
                column: "MenuViewName",
                value: "Protocols");

            migrationBuilder.UpdateData(
                table: "Menus",
                keyColumn: "menuID",
                keyValue: 3,
                columns: new[] { "MenuDescription", "MenuViewName" },
                values: new object[] { "Operation", "Operation" });

            migrationBuilder.UpdateData(
                table: "Menus",
                keyColumn: "menuID",
                keyValue: 4,
                column: "MenuViewName",
                value: "Biomarkers");

            migrationBuilder.UpdateData(
                table: "Menus",
                keyColumn: "menuID",
                keyValue: 5,
                columns: new[] { "MenuDescription", "MenuViewName" },
                values: new object[] { "LabManagement", "Lab Management" });

            migrationBuilder.UpdateData(
                table: "Menus",
                keyColumn: "menuID",
                keyValue: 6,
                column: "MenuViewName",
                value: "Accounting");

            migrationBuilder.UpdateData(
                table: "Menus",
                keyColumn: "menuID",
                keyValue: 7,
                column: "MenuViewName",
                value: "Expenses");

            migrationBuilder.UpdateData(
                table: "Menus",
                keyColumn: "menuID",
                keyValue: 8,
                column: "MenuViewName",
                value: "Users");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Menus",
                keyColumn: "menuID",
                keyValue: 1,
                columns: new[] { "MenuDescription", "MenuViewName" },
                values: new object[] { "Orders & Inventory", null });

            migrationBuilder.UpdateData(
                table: "Menus",
                keyColumn: "menuID",
                keyValue: 2,
                column: "MenuViewName",
                value: null);

            migrationBuilder.UpdateData(
                table: "Menus",
                keyColumn: "menuID",
                keyValue: 3,
                columns: new[] { "MenuDescription", "MenuViewName" },
                values: new object[] { "Operations", null });

            migrationBuilder.UpdateData(
                table: "Menus",
                keyColumn: "menuID",
                keyValue: 4,
                column: "MenuViewName",
                value: null);

            migrationBuilder.UpdateData(
                table: "Menus",
                keyColumn: "menuID",
                keyValue: 5,
                columns: new[] { "MenuDescription", "MenuViewName" },
                values: new object[] { "Lab Management", null });

            migrationBuilder.UpdateData(
                table: "Menus",
                keyColumn: "menuID",
                keyValue: 6,
                column: "MenuViewName",
                value: null);

            migrationBuilder.UpdateData(
                table: "Menus",
                keyColumn: "menuID",
                keyValue: 7,
                column: "MenuViewName",
                value: null);

            migrationBuilder.UpdateData(
                table: "Menus",
                keyColumn: "menuID",
                keyValue: 8,
                column: "MenuViewName",
                value: null);
        }
    }
}
