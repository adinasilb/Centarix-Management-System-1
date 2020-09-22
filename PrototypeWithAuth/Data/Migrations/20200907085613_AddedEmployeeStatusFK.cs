using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class AddedEmployeeStatusFK : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmployeeStatus",
                table: "SalariedEmployees");

            migrationBuilder.DropColumn(
                name: "EmployeeStatus",
                table: "Freelancers");

            migrationBuilder.AddColumn<int>(
                name: "EmployeeStatusID",
                table: "SalariedEmployees",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "EmployeeStatusID",
                table: "Freelancers",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_SalariedEmployees_EmployeeStatusID",
                table: "SalariedEmployees",
                column: "EmployeeStatusID");

            migrationBuilder.CreateIndex(
                name: "IX_Freelancers_EmployeeStatusID",
                table: "Freelancers",
                column: "EmployeeStatusID");

            migrationBuilder.AddForeignKey(
                name: "FK_Freelancers_EmployeeStatuses_EmployeeStatusID",
                table: "Freelancers",
                column: "EmployeeStatusID",
                principalTable: "EmployeeStatuses",
                principalColumn: "EmployeeStatusID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SalariedEmployees_EmployeeStatuses_EmployeeStatusID",
                table: "SalariedEmployees",
                column: "EmployeeStatusID",
                principalTable: "EmployeeStatuses",
                principalColumn: "EmployeeStatusID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Freelancers_EmployeeStatuses_EmployeeStatusID",
                table: "Freelancers");

            migrationBuilder.DropForeignKey(
                name: "FK_SalariedEmployees_EmployeeStatuses_EmployeeStatusID",
                table: "SalariedEmployees");

            migrationBuilder.DropIndex(
                name: "IX_SalariedEmployees_EmployeeStatusID",
                table: "SalariedEmployees");

            migrationBuilder.DropIndex(
                name: "IX_Freelancers_EmployeeStatusID",
                table: "Freelancers");

            migrationBuilder.DropColumn(
                name: "EmployeeStatusID",
                table: "SalariedEmployees");

            migrationBuilder.DropColumn(
                name: "EmployeeStatusID",
                table: "Freelancers");

            migrationBuilder.AddColumn<int>(
                name: "EmployeeStatus",
                table: "SalariedEmployees",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "EmployeeStatus",
                table: "Freelancers",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
