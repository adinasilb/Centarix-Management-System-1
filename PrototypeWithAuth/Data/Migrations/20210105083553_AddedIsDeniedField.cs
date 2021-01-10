using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class AddedIsDeniedField : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDenied",
                table: "EmployeeHoursAwaitingApprovals",
                nullable: true);
            migrationBuilder.UpdateData("EmployeeHoursAwaitingApprovals", "EmployeeHoursAwaitingApprovalID", "*", "IsDenied", false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDenied",
                table: "EmployeeHoursAwaitingApprovals");
        }
    }
}
