using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class testNoDatetime : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "EmployeeStatuses",
                keyColumn: "EmployeeStatusID",
                keyValue: 1,
                column: "LastCentarixIDTimeStamp",
                value: new DateTime(2020, 12, 24, 14, 43, 21, 636, DateTimeKind.Local).AddTicks(7685));

            migrationBuilder.UpdateData(
                table: "EmployeeStatuses",
                keyColumn: "EmployeeStatusID",
                keyValue: 2,
                column: "LastCentarixIDTimeStamp",
                value: new DateTime(2020, 12, 24, 14, 43, 21, 639, DateTimeKind.Local).AddTicks(4090));

            migrationBuilder.UpdateData(
                table: "EmployeeStatuses",
                keyColumn: "EmployeeStatusID",
                keyValue: 3,
                column: "LastCentarixIDTimeStamp",
                value: new DateTime(2020, 12, 24, 14, 43, 21, 639, DateTimeKind.Local).AddTicks(4124));

            migrationBuilder.UpdateData(
                table: "EmployeeStatuses",
                keyColumn: "EmployeeStatusID",
                keyValue: 4,
                column: "LastCentarixIDTimeStamp",
                value: new DateTime(2020, 12, 24, 14, 43, 21, 639, DateTimeKind.Local).AddTicks(4129));
        }
    }
}
