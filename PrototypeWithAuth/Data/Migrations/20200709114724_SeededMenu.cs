using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class SeededMenu : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Menus",
                keyColumn: "menuID",
                keyValue: 5,
                columns: new[] { "ActionName", "ControllerName", "MenuDescription", "MenuImageURL" },
                values: new object[] { "", "", "TimeKeeper", "/images/css/main_menu_icons/expenses.png" });

            migrationBuilder.UpdateData(
                table: "Menus",
                keyColumn: "menuID",
                keyValue: 6,
                columns: new[] { "ActionName", "ControllerName", "MenuDescription", "MenuImageURL" },
                values: new object[] { "IndexForLabManage", "Vendors", "Lab Management", "/images/css/main_menu_icons/lab.png" });

            migrationBuilder.UpdateData(
                table: "Menus",
                keyColumn: "menuID",
                keyValue: 7,
                columns: new[] { "ActionName", "MenuDescription", "MenuImageURL" },
                values: new object[] { "ToPay", "Accounting", "/images/css/main_menu_icons/accounting.png" });

            migrationBuilder.UpdateData(
                table: "Menus",
                keyColumn: "menuID",
                keyValue: 8,
                columns: new[] { "ActionName", "ControllerName", "MenuDescription", "MenuImageURL" },
                values: new object[] { "ExpensesList", "ParentRequests", "Expenses", "/images/css/main_menu_icons/expenses.png" });

            migrationBuilder.InsertData(
                table: "Menus",
                columns: new[] { "menuID", "ActionName", "ControllerName", "MenuDescription", "MenuImageURL" },
                values: new object[,]
                {
                    { 9, "", "", "Income", "/images/css/main_menu_icons/users.png" },
                    { 10, "RegisterUser", "Admin", "Users", "/images/css/main_menu_icons/users.png" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Menus",
                keyColumn: "menuID",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Menus",
                keyColumn: "menuID",
                keyValue: 10);

            migrationBuilder.UpdateData(
                table: "Menus",
                keyColumn: "menuID",
                keyValue: 5,
                columns: new[] { "ActionName", "ControllerName", "MenuDescription", "MenuImageURL" },
                values: new object[] { "IndexForLabManage", "Vendors", "Lab Management", "/images/css/main_menu_icons/lab.png" });

            migrationBuilder.UpdateData(
                table: "Menus",
                keyColumn: "menuID",
                keyValue: 6,
                columns: new[] { "ActionName", "ControllerName", "MenuDescription", "MenuImageURL" },
                values: new object[] { "ToPay", "ParentRequests", "Accounting", "/images/css/main_menu_icons/accounting.png" });

            migrationBuilder.UpdateData(
                table: "Menus",
                keyColumn: "menuID",
                keyValue: 7,
                columns: new[] { "ActionName", "MenuDescription", "MenuImageURL" },
                values: new object[] { "ExpensesList", "Expenses", "/images/css/main_menu_icons/expenses.png" });

            migrationBuilder.UpdateData(
                table: "Menus",
                keyColumn: "menuID",
                keyValue: 8,
                columns: new[] { "ActionName", "ControllerName", "MenuDescription", "MenuImageURL" },
                values: new object[] { "RegisterUser", "Admin", "Users", "/images/css/main_menu_icons/users.png" });
        }
    }
}
