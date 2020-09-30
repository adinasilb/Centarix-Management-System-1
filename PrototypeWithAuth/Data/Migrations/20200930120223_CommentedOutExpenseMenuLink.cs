using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class CommentedOutExpenseMenuLink : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Menus",
                keyColumn: "menuID",
                keyValue: 8,
                columns: new[] { "ActionName", "ControllerName" },
                values: new object[] { "", "" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Menus",
                keyColumn: "menuID",
                keyValue: 8,
                columns: new[] { "ActionName", "ControllerName" },
                values: new object[] { "ExpensesList", "ParentRequests" });
        }
    }
}
