using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class addedUserToEmployeeStatus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "EmployeeStatuses",
                columns: new[] { "EmployeeStatusID", "Description" },
                values: new object[] { 4, "User" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "EmployeeStatuses",
                keyColumn: "EmployeeStatusID",
                keyValue: 4);
        }
    }
}
