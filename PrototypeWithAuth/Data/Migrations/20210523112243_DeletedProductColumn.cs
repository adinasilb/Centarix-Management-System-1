using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class DeletedProductColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_EmployeeHoursAwaitingApprovals_EmployeeHoursID",
                table: "EmployeeHoursAwaitingApprovals");

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Products",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeHoursAwaitingApprovals_EmployeeHoursID",
                table: "EmployeeHoursAwaitingApprovals",
                column: "EmployeeHoursID",
                unique: true,
                filter: "[EmployeeHoursID] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_EmployeeHoursAwaitingApprovals_EmployeeHoursID",
                table: "EmployeeHoursAwaitingApprovals");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Products");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeHoursAwaitingApprovals_EmployeeHoursID",
                table: "EmployeeHoursAwaitingApprovals",
                column: "EmployeeHoursID");
        }
    }
}
