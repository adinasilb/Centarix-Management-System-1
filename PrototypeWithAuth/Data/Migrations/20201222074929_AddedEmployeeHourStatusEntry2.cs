using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class AddedEmployeeHourStatusEntry2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeHours_EmployeeHoursStatuses_EmployeeHoursStatusID",
                table: "EmployeeHours");

            migrationBuilder.DropIndex(
                name: "IX_EmployeeHours_EmployeeHoursStatusID",
                table: "EmployeeHours");

            migrationBuilder.DropColumn(
                name: "EmployeeHoursStatusID",
                table: "EmployeeHours");

            migrationBuilder.AddColumn<int>(
                name: "EmployeeHoursStatusEntry1ID",
                table: "EmployeeHours",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeHours_EmployeeHoursStatusEntry1ID",
                table: "EmployeeHours",
                column: "EmployeeHoursStatusEntry1ID");

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeHours_EmployeeHoursStatuses_EmployeeHoursStatusEntry1ID",
                table: "EmployeeHours",
                column: "EmployeeHoursStatusEntry1ID",
                principalTable: "EmployeeHoursStatuses",
                principalColumn: "EmployeeHoursStatusID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeHours_EmployeeHoursStatuses_EmployeeHoursStatusEntry1ID",
                table: "EmployeeHours");

            migrationBuilder.DropIndex(
                name: "IX_EmployeeHours_EmployeeHoursStatusEntry1ID",
                table: "EmployeeHours");

            migrationBuilder.DropColumn(
                name: "EmployeeHoursStatusEntry1ID",
                table: "EmployeeHours");

            migrationBuilder.AddColumn<int>(
                name: "EmployeeHoursStatusID",
                table: "EmployeeHours",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeHours_EmployeeHoursStatusID",
                table: "EmployeeHours",
                column: "EmployeeHoursStatusID");

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeHours_EmployeeHoursStatuses_EmployeeHoursStatusID",
                table: "EmployeeHours",
                column: "EmployeeHoursStatusID",
                principalTable: "EmployeeHoursStatuses",
                principalColumn: "EmployeeHoursStatusID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
