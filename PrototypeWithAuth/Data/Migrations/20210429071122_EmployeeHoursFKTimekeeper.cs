using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class EmployeeHoursFKTimekeeper : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_TimekeeperNotifications_EmployeeHoursID",
                table: "TimekeeperNotifications",
                column: "EmployeeHoursID");

            migrationBuilder.AddForeignKey(
                name: "FK_TimekeeperNotifications_EmployeeHours_EmployeeHoursID",
                table: "TimekeeperNotifications",
                column: "EmployeeHoursID",
                principalTable: "EmployeeHours",
                principalColumn: "EmployeeHoursID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TimekeeperNotifications_EmployeeHours_EmployeeHoursID",
                table: "TimekeeperNotifications");

            migrationBuilder.DropIndex(
                name: "IX_TimekeeperNotifications_EmployeeHoursID",
                table: "TimekeeperNotifications");
        }
    }
}
