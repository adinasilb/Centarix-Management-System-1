using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class seedMenu : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Menus",
                columns: new[] { "menuID", "ActionName", "ControllerName", "MenuDescription", "MenuImageURL" },
                values: new object[,]
                {
                    { 1, "Index", "Requests", "Orders & Inventory", "/images/css/main_menu_icons/inventory.png" },
                    { 2, "", "", "Protocols", "/images/css/main_menu_icons/protocols.png" },
                    { 3, "", "", "Operations", "/images/css/main_menu_icons/operation.png" },
                    { 4, "", "", "Biomarkers", "/images/css/main_menu_icons/biomarkers.png" },
                    { 5, "IndexForLabManage", "Vendors", "Lab Management", "/images/css/main_menu_icons/lab.png" },
                    { 6, "ToPay", "ParentRequests", "Accounting", "/images/css/main_menu_icons/accounting.png" },
                    { 7, "ExpensesList", "ParentRequests", "Expenses", "/images/css/main_menu_icons/expenses.png" },
                    { 8, "RegisterUser", "Admin", "Users", "/images/css/main_menu_icons/users.png" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
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
        }
    }
}
