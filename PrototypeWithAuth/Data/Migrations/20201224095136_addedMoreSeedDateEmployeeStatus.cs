using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class addedMoreSeedDateEmployeeStatus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "EmployeeStatuses",
                keyColumn: "EmployeeStatusID",
                keyValue: 3,
                column: "Abbreviation",
                value: "A");

            migrationBuilder.UpdateData(
                table: "EmployeeStatuses",
                keyColumn: "EmployeeStatusID",
                keyValue: 4,
                column: "Abbreviation",
                value: "U");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "EmployeeStatuses",
                keyColumn: "EmployeeStatusID",
                keyValue: 3,
                column: "Abbreviation",
                value: null);

            migrationBuilder.UpdateData(
                table: "EmployeeStatuses",
                keyColumn: "EmployeeStatusID",
                keyValue: 4,
                column: "Abbreviation",
                value: null);
        }
    }
}
