using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class removeMenuItems : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Menus",
                keyColumn: "menuID",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Menus",
                keyColumn: "menuID",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Menus",
                keyColumn: "menuID",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Menus",
                keyColumn: "menuID",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Menus",
                keyColumn: "menuID",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Menus",
                keyColumn: "menuID",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Menus",
                keyColumn: "menuID",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Menus",
                keyColumn: "menuID",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Menus",
                keyColumn: "menuID",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Menus",
                keyColumn: "menuID",
                keyValue: 10);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Menus",
                columns: new[] { "menuID", "ActionName", "ControllerName", "MenuDescription", "MenuImageURL", "MenuViewName" },
                values: new object[,]
                {
                    { 1, "Index", "Requests", "OrdersAndInventory", "/images/css/main_menu_icons/inventory.png", "Orders & Inventory" },
                    { 2, "", "", "Protocols", "/images/css/main_menu_icons/protocols.png", "Protocols" },
                    { 3, "", "", "Operation", "/images/css/main_menu_icons/operation.png", "Operation" },
                    { 4, "", "", "Biomarkers", "/images/css/main_menu_icons/biomarkers.png", "Biomarkers" },
                    { 5, "", "", "TimeKeeper", "/images/css/main_menu_icons/timekeeper.png", "TimeKeeper" },
                    { 6, "IndexForLabManage", "Vendors", "LabManagement", "/images/css/main_menu_icons/lab.png", "Lab Management" },
                    { 7, "ToPay", "ParentRequests", "Accounting", "/images/css/main_menu_icons/accounting.png", "Accounting" },
                    { 8, "ExpensesList", "ParentRequests", "Expenses", "/images/css/main_menu_icons/expenses.png", "Expenses" },
                    { 9, "", "", "Income", "/images/css/main_menu_icons/income.png", "Income" },
                    { 10, "Index", "Admin", "Users", "/images/css/main_menu_icons/users.png", "Users" }
                });
        }
    }
}
