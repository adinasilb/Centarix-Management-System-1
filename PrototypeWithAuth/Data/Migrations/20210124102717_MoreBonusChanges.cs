using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class MoreBonusChanges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "isBonus",
                table: "EmployeeHours",
                newName: "IsBonus");

            migrationBuilder.AddColumn<bool>(
                name: "IsBonus",
                table: "EmployeeHoursAwaitingApprovals",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsBonus",
                table: "EmployeeHoursAwaitingApprovals");

            migrationBuilder.RenameColumn(
                name: "IsBonus",
                table: "EmployeeHours",
                newName: "isBonus");
        }
    }
}
