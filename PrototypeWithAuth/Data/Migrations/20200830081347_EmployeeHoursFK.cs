using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class EmployeeHoursFK : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "EmployeeID",
                table: "EmployeeHours",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SickDays",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "VacationDays",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeHours_EmployeeID",
                table: "EmployeeHours",
                column: "EmployeeID");

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeHours_AspNetUsers_EmployeeID",
                table: "EmployeeHours",
                column: "EmployeeID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeHours_AspNetUsers_EmployeeID",
                table: "EmployeeHours");

            migrationBuilder.DropIndex(
                name: "IX_EmployeeHours_EmployeeID",
                table: "EmployeeHours");

            migrationBuilder.DropColumn(
                name: "EmployeeID",
                table: "EmployeeHours");

            migrationBuilder.DropColumn(
                name: "SickDays",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "VacationDays",
                table: "AspNetUsers");
        }
    }
}
