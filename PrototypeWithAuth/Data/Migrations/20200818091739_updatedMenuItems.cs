using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class updatedMenuItems : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Menus",
                keyColumn: "menuID",
                keyValue: 5,
                column: "MenuViewName",
                value: "Time Keeper");

            migrationBuilder.UpdateData(
                table: "Menus",
                keyColumn: "menuID",
                keyValue: 7,
                columns: new[] { "ActionName", "ControllerName" },
                values: new object[] { "AccountingPayments", "Requests" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Menus",
                keyColumn: "menuID",
                keyValue: 5,
                column: "MenuViewName",
                value: "TimeKeeper");

            migrationBuilder.UpdateData(
                table: "Menus",
                keyColumn: "menuID",
                keyValue: 7,
                columns: new[] { "ActionName", "ControllerName" },
                values: new object[] { "Payments", "ParentRequests" });
        }
    }
}
