using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class addedIsCurrentToCentarixIDAndUpdatedDateTimeOfMED : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsCurrent",
                table: "CentarixIDs",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "EmployeeStatuses",
                keyColumn: "EmployeeStatusID",
                keyValue: 1,
                column: "LastCentarixIDTimeStamp",
                value: new DateTime(2020, 12, 24, 12, 24, 31, 342, DateTimeKind.Local).AddTicks(9771));

            migrationBuilder.UpdateData(
                table: "EmployeeStatuses",
                keyColumn: "EmployeeStatusID",
                keyValue: 2,
                column: "LastCentarixIDTimeStamp",
                value: new DateTime(2020, 12, 24, 12, 24, 31, 345, DateTimeKind.Local).AddTicks(7746));

            migrationBuilder.UpdateData(
                table: "EmployeeStatuses",
                keyColumn: "EmployeeStatusID",
                keyValue: 3,
                column: "LastCentarixIDTimeStamp",
                value: new DateTime(2020, 12, 24, 12, 24, 31, 345, DateTimeKind.Local).AddTicks(7788));

            migrationBuilder.UpdateData(
                table: "EmployeeStatuses",
                keyColumn: "EmployeeStatusID",
                keyValue: 4,
                column: "LastCentarixIDTimeStamp",
                value: new DateTime(2020, 12, 24, 12, 24, 31, 345, DateTimeKind.Local).AddTicks(7793));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsCurrent",
                table: "CentarixIDs");

            migrationBuilder.UpdateData(
                table: "EmployeeStatuses",
                keyColumn: "EmployeeStatusID",
                keyValue: 1,
                column: "LastCentarixIDTimeStamp",
                value: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "EmployeeStatuses",
                keyColumn: "EmployeeStatusID",
                keyValue: 2,
                column: "LastCentarixIDTimeStamp",
                value: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "EmployeeStatuses",
                keyColumn: "EmployeeStatusID",
                keyValue: 3,
                column: "LastCentarixIDTimeStamp",
                value: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "EmployeeStatuses",
                keyColumn: "EmployeeStatusID",
                keyValue: 4,
                column: "LastCentarixIDTimeStamp",
                value: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
