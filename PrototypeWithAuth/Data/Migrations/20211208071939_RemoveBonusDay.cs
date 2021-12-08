using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class RemoveBonusDay : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsBonus",
                table: "EmployeeHoursAwaitingApprovals");

            migrationBuilder.DropColumn(
                name: "IsBonus",
                table: "EmployeeHours");

            migrationBuilder.DropColumn(
                name: "BonusSickDays",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "BonusVacationDays",
                table: "AspNetUsers");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsBonus",
                table: "EmployeeHoursAwaitingApprovals",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsBonus",
                table: "EmployeeHours",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<double>(
                name: "BonusSickDays",
                table: "AspNetUsers",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "BonusVacationDays",
                table: "AspNetUsers",
                type: "float",
                nullable: true);
        }
    }
}
