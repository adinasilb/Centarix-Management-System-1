using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class AddedEmployeHoursForeignKey2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EmployeeHoursID",
                table: "EmployeeHoursAwaitingApprovals",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeHoursAwaitingApprovals_EmployeeHoursID",
                table: "EmployeeHoursAwaitingApprovals",
                column: "EmployeeHoursID");

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeHoursAwaitingApprovals_EmployeeHours_EmployeeHoursID",
                table: "EmployeeHoursAwaitingApprovals",
                column: "EmployeeHoursID",
                principalTable: "EmployeeHours",
                principalColumn: "EmployeeHoursID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeHoursAwaitingApprovals_EmployeeHours_EmployeeHoursID",
                table: "EmployeeHoursAwaitingApprovals");

            migrationBuilder.DropIndex(
                name: "IX_EmployeeHoursAwaitingApprovals_EmployeeHoursID",
                table: "EmployeeHoursAwaitingApprovals");

            migrationBuilder.DropColumn(
                name: "EmployeeHoursID",
                table: "EmployeeHoursAwaitingApprovals");
        }
    }
}
