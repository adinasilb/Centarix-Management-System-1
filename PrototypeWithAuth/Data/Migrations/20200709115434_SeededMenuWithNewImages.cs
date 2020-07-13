using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class SeededMenuWithNewImages : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Menus",
                keyColumn: "menuID",
                keyValue: 5,
                column: "MenuImageURL",
                value: "/images/css/main_menu_icons/timekeeper.png");

            migrationBuilder.UpdateData(
                table: "Menus",
                keyColumn: "menuID",
                keyValue: 9,
                column: "MenuImageURL",
                value: "/images/css/main_menu_icons/Income.png");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Menus",
                keyColumn: "menuID",
                keyValue: 5,
                column: "MenuImageURL",
                value: "/images/css/main_menu_icons/expenses.png");

            migrationBuilder.UpdateData(
                table: "Menus",
                keyColumn: "menuID",
                keyValue: 9,
                column: "MenuImageURL",
                value: "/images/css/main_menu_icons/users.png");
        }
    }
}
