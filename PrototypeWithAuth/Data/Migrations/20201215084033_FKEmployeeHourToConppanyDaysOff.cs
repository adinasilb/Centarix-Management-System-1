using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class FKEmployeeHourToConppanyDaysOff : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CompanyDayOffID",
                table: "EmployeeHours",
                nullable: true);

            migrationBuilder.InsertData(
                table: "OffDayTypes",
                columns: new[] { "OffDayTypeID", "Description" },
                values: new object[] { 3, "Company Day Off" });

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeHours_CompanyDayOffID",
                table: "EmployeeHours",
                column: "CompanyDayOffID");

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeHours_CompanyDayOffs_CompanyDayOffID",
                table: "EmployeeHours",
                column: "CompanyDayOffID",
                principalTable: "CompanyDayOffs",
                principalColumn: "CompanyDayOffID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeHours_CompanyDayOffs_CompanyDayOffID",
                table: "EmployeeHours");

            migrationBuilder.DropIndex(
                name: "IX_EmployeeHours_CompanyDayOffID",
                table: "EmployeeHours");

            migrationBuilder.DeleteData(
                table: "OffDayTypes",
                keyColumn: "OffDayTypeID",
                keyValue: 3);

            migrationBuilder.DropColumn(
                name: "CompanyDayOffID",
                table: "EmployeeHours");
        }
    }
}
