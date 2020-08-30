using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class AddedTimekeeperAction : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Menus",
                keyColumn: "menuID",
                keyValue: 5,
                columns: new[] { "ActionName", "ControllerName", "MenuViewName" },
                values: new object[] { "ReportHours", "Timekeeper", "Timekeeper" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Menus",
                keyColumn: "menuID",
                keyValue: 5,
                columns: new[] { "ActionName", "ControllerName", "MenuViewName" },
                values: new object[] { "", "", "Time Keeper" });
        }
    }
}
