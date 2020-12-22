using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class AddedEmployeeHourStatusEntry22 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EmployeeHoursStatusEntry2ID",
                table: "EmployeeHours",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeHours_EmployeeHoursStatusEntry2ID",
                table: "EmployeeHours",
                column: "EmployeeHoursStatusEntry2ID");

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeHours_EmployeeHoursStatuses_EmployeeHoursStatusEntry2ID",
                table: "EmployeeHours",
                column: "EmployeeHoursStatusEntry2ID",
                principalTable: "EmployeeHoursStatuses",
                principalColumn: "EmployeeHoursStatusID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeHours_EmployeeHoursStatuses_EmployeeHoursStatusEntry2ID",
                table: "EmployeeHours");

            migrationBuilder.DropIndex(
                name: "IX_EmployeeHours_EmployeeHoursStatusEntry2ID",
                table: "EmployeeHours");

            migrationBuilder.DropColumn(
                name: "EmployeeHoursStatusEntry2ID",
                table: "EmployeeHours");
        }
    }
}
