using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class MovedFieldsFromEmployeeToSalaried : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_SalariedEmployees_EmployeeId",
                table: "SalariedEmployees");

            migrationBuilder.DropIndex(
                name: "IX_Freelancers_EmployeeId",
                table: "Freelancers");

            migrationBuilder.DropColumn(
                name: "HoursPerDay",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<double>(
                name: "HoursPerDay",
                table: "SalariedEmployees",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.CreateIndex(
                name: "IX_SalariedEmployees_EmployeeId",
                table: "SalariedEmployees",
                column: "EmployeeId",
                unique: true,
                filter: "[EmployeeId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Freelancers_EmployeeId",
                table: "Freelancers",
                column: "EmployeeId",
                unique: true,
                filter: "[EmployeeId] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_SalariedEmployees_EmployeeId",
                table: "SalariedEmployees");

            migrationBuilder.DropIndex(
                name: "IX_Freelancers_EmployeeId",
                table: "Freelancers");

            migrationBuilder.DropColumn(
                name: "HoursPerDay",
                table: "SalariedEmployees");

            migrationBuilder.AddColumn<double>(
                name: "HoursPerDay",
                table: "AspNetUsers",
                type: "float",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SalariedEmployees_EmployeeId",
                table: "SalariedEmployees",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_Freelancers_EmployeeId",
                table: "Freelancers",
                column: "EmployeeId");
        }
    }
}
