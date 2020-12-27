using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class employeeStatusFKOnCentarixID : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_CentarixIDs_EmployeeStatusID",
                table: "CentarixIDs",
                column: "EmployeeStatusID");

            migrationBuilder.AddForeignKey(
                name: "FK_CentarixIDs_EmployeeStatuses_EmployeeStatusID",
                table: "CentarixIDs",
                column: "EmployeeStatusID",
                principalTable: "EmployeeStatuses",
                principalColumn: "EmployeeStatusID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CentarixIDs_EmployeeStatuses_EmployeeStatusID",
                table: "CentarixIDs");

            migrationBuilder.DropIndex(
                name: "IX_CentarixIDs_EmployeeStatusID",
                table: "CentarixIDs");
        }
    }
}
