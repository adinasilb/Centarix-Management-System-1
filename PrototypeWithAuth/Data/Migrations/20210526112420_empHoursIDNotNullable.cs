using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class empHoursIDNotNullable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_EmployeeHoursAwaitingApprovals_EmployeeHoursID",
                table: "EmployeeHoursAwaitingApprovals");

            migrationBuilder.AlterColumn<bool>(
                name: "IncludeVAT",
                table: "Requests",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<int>(
                name: "EmployeeHoursID",
                table: "EmployeeHoursAwaitingApprovals",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeHoursAwaitingApprovals_EmployeeHoursID",
                table: "EmployeeHoursAwaitingApprovals",
                column: "EmployeeHoursID",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_EmployeeHoursAwaitingApprovals_EmployeeHoursID",
                table: "EmployeeHoursAwaitingApprovals");

            migrationBuilder.AlterColumn<bool>(
                name: "IncludeVAT",
                table: "Requests",
                type: "bit",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool));

            migrationBuilder.AlterColumn<int>(
                name: "EmployeeHoursID",
                table: "EmployeeHoursAwaitingApprovals",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeHoursAwaitingApprovals_EmployeeHoursID",
                table: "EmployeeHoursAwaitingApprovals",
                column: "EmployeeHoursID",
                unique: true,
                filter: "[EmployeeHoursID] IS NOT NULL");
        }
    }
}
