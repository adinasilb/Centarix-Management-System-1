using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class EmployeeStatusData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "EmployeeStatuses",
                columns: new[] { "EmployeeStatusID", "Description" },
                values: new object[] { 1, "SalariedEmployee" });

            migrationBuilder.InsertData(
                table: "EmployeeStatuses",
                columns: new[] { "EmployeeStatusID", "Description" },
                values: new object[] { 2, "Freelancer" });

            migrationBuilder.InsertData(
                table: "EmployeeStatuses",
                columns: new[] { "EmployeeStatusID", "Description" },
                values: new object[] { 3, "Advisor" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "EmployeeStatuses",
                keyColumn: "EmployeeStatusID",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "EmployeeStatuses",
                keyColumn: "EmployeeStatusID",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "EmployeeStatuses",
                keyColumn: "EmployeeStatusID",
                keyValue: 3);
        }
    }
}
