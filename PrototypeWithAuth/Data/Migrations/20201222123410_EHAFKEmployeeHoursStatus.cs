using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class EHAFKEmployeeHoursStatus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeHoursAwaitingApprovals_EmployeeHoursStatuses_EmployeeHoursStatusID",
                table: "EmployeeHoursAwaitingApprovals");

            migrationBuilder.DropIndex(
                name: "IX_EmployeeHoursAwaitingApprovals_EmployeeHoursStatusID",
                table: "EmployeeHoursAwaitingApprovals");

            migrationBuilder.DropColumn(
                name: "EmployeeHoursStatusID",
                table: "EmployeeHoursAwaitingApprovals");

            migrationBuilder.AddColumn<int>(
                name: "EmployeeHoursStatusEntry1ID",
                table: "EmployeeHoursAwaitingApprovals",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "EmployeeHoursStatusEntry2ID",
                table: "EmployeeHoursAwaitingApprovals",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeHoursAwaitingApprovals_EmployeeHoursStatusEntry1ID",
                table: "EmployeeHoursAwaitingApprovals",
                column: "EmployeeHoursStatusEntry1ID");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeHoursAwaitingApprovals_EmployeeHoursStatusEntry2ID",
                table: "EmployeeHoursAwaitingApprovals",
                column: "EmployeeHoursStatusEntry2ID");

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeHoursAwaitingApprovals_EmployeeHoursStatuses_EmployeeHoursStatusEntry1ID",
                table: "EmployeeHoursAwaitingApprovals",
                column: "EmployeeHoursStatusEntry1ID",
                principalTable: "EmployeeHoursStatuses",
                principalColumn: "EmployeeHoursStatusID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeHoursAwaitingApprovals_EmployeeHoursStatuses_EmployeeHoursStatusEntry2ID",
                table: "EmployeeHoursAwaitingApprovals",
                column: "EmployeeHoursStatusEntry2ID",
                principalTable: "EmployeeHoursStatuses",
                principalColumn: "EmployeeHoursStatusID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeHoursAwaitingApprovals_EmployeeHoursStatuses_EmployeeHoursStatusEntry1ID",
                table: "EmployeeHoursAwaitingApprovals");

            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeHoursAwaitingApprovals_EmployeeHoursStatuses_EmployeeHoursStatusEntry2ID",
                table: "EmployeeHoursAwaitingApprovals");

            migrationBuilder.DropIndex(
                name: "IX_EmployeeHoursAwaitingApprovals_EmployeeHoursStatusEntry1ID",
                table: "EmployeeHoursAwaitingApprovals");

            migrationBuilder.DropIndex(
                name: "IX_EmployeeHoursAwaitingApprovals_EmployeeHoursStatusEntry2ID",
                table: "EmployeeHoursAwaitingApprovals");

            migrationBuilder.DropColumn(
                name: "EmployeeHoursStatusEntry1ID",
                table: "EmployeeHoursAwaitingApprovals");

            migrationBuilder.DropColumn(
                name: "EmployeeHoursStatusEntry2ID",
                table: "EmployeeHoursAwaitingApprovals");

            migrationBuilder.AddColumn<int>(
                name: "EmployeeHoursStatusID",
                table: "EmployeeHoursAwaitingApprovals",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeHoursAwaitingApprovals_EmployeeHoursStatusID",
                table: "EmployeeHoursAwaitingApprovals",
                column: "EmployeeHoursStatusID");

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeHoursAwaitingApprovals_EmployeeHoursStatuses_EmployeeHoursStatusID",
                table: "EmployeeHoursAwaitingApprovals",
                column: "EmployeeHoursStatusID",
                principalTable: "EmployeeHoursStatuses",
                principalColumn: "EmployeeHoursStatusID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
