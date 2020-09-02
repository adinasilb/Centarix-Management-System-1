using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class AddedTempTableForEMployeeHours : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "OffDayTypes",
                keyColumn: "OffDayTypeID",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "OffDayTypes",
                keyColumn: "OffDayTypeID",
                keyValue: 2);

            migrationBuilder.AlterColumn<DateTime>(
                name: "Entry1",
                table: "EmployeeHours",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.InsertData(
                table: "EmployeeHoursStatuses",
                columns: new[] { "EmployeeHoursStatusID", "Description" },
                values: new object[] { 1, "Work from home" });

            migrationBuilder.InsertData(
                table: "EmployeeHoursStatuses",
                columns: new[] { "EmployeeHoursStatusID", "Description" },
                values: new object[] { 2, "Edit existing hours" });

            migrationBuilder.InsertData(
                table: "EmployeeHoursStatuses",
                columns: new[] { "EmployeeHoursStatusID", "Description" },
                values: new object[] { 3, "Forgot to report" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "EmployeeHoursStatuses",
                keyColumn: "EmployeeHoursStatusID",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "EmployeeHoursStatuses",
                keyColumn: "EmployeeHoursStatusID",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "EmployeeHoursStatuses",
                keyColumn: "EmployeeHoursStatusID",
                keyValue: 3);

            migrationBuilder.AlterColumn<DateTime>(
                name: "Entry1",
                table: "EmployeeHours",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldNullable: true);

            migrationBuilder.InsertData(
                table: "OffDayTypes",
                columns: new[] { "OffDayTypeID", "Description" },
                values: new object[] { 1, "SickDay" });

            migrationBuilder.InsertData(
                table: "OffDayTypes",
                columns: new[] { "OffDayTypeID", "Description" },
                values: new object[] { 2, " VacationDay" });
        }
    }
}
