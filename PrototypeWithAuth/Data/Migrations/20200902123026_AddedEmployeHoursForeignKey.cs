using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class AddedEmployeHoursForeignKey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_EmployeeHoursAwaitingApprovals",
                table: "EmployeeHoursAwaitingApprovals");

            migrationBuilder.DropColumn(
                name: "EmployeeHoursID",
                table: "EmployeeHoursAwaitingApprovals");

            migrationBuilder.AddColumn<int>(
                name: "EmployeeHoursAwaitingApprovalID",
                table: "EmployeeHoursAwaitingApprovals",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EmployeeHoursAwaitingApprovals",
                table: "EmployeeHoursAwaitingApprovals",
                column: "EmployeeHoursAwaitingApprovalID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_EmployeeHoursAwaitingApprovals",
                table: "EmployeeHoursAwaitingApprovals");

            migrationBuilder.DropColumn(
                name: "EmployeeHoursAwaitingApprovalID",
                table: "EmployeeHoursAwaitingApprovals");

            migrationBuilder.AddColumn<int>(
                name: "EmployeeHoursID",
                table: "EmployeeHoursAwaitingApprovals",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EmployeeHoursAwaitingApprovals",
                table: "EmployeeHoursAwaitingApprovals",
                column: "EmployeeHoursID");
        }
    }
}
