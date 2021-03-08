using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class PartialOffHourEAModelExtensionData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeHours_PartialOffDayType_PartialOffDayTypeID",
                table: "EmployeeHours");

            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeHoursAwaitingApprovals_PartialOffDayType_PartialOffDayTypeID",
                table: "EmployeeHoursAwaitingApprovals");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PartialOffDayType",
                table: "PartialOffDayType");

            migrationBuilder.DropColumn(
                name: "OffDayTypeID",
                table: "PartialOffDayType");

            migrationBuilder.RenameTable(
                name: "PartialOffDayType",
                newName: "PartialOffDayTypes");

            migrationBuilder.AlterColumn<TimeSpan>(
                name: "PartialOffDayHours",
                table: "EmployeeHoursAwaitingApprovals",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<TimeSpan>(
                name: "PartialOffDayHours",
                table: "EmployeeHours",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<int>(
                name: "PartialOffDayTypeID",
                table: "PartialOffDayTypes",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PartialOffDayTypes",
                table: "PartialOffDayTypes",
                column: "PartialOffDayTypeID");

            migrationBuilder.InsertData(
                table: "PartialOffDayTypes",
                columns: new[] { "PartialOffDayTypeID", "Description" },
                values: new object[] { 1, "Partial Sick Day" });

            migrationBuilder.InsertData(
                table: "PartialOffDayTypes",
                columns: new[] { "PartialOffDayTypeID", "Description" },
                values: new object[] { 2, "Partial Vacation Day" });

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeHours_PartialOffDayTypes_PartialOffDayTypeID",
                table: "EmployeeHours",
                column: "PartialOffDayTypeID",
                principalTable: "PartialOffDayTypes",
                principalColumn: "PartialOffDayTypeID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeHoursAwaitingApprovals_PartialOffDayTypes_PartialOffDayTypeID",
                table: "EmployeeHoursAwaitingApprovals",
                column: "PartialOffDayTypeID",
                principalTable: "PartialOffDayTypes",
                principalColumn: "PartialOffDayTypeID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeHours_PartialOffDayTypes_PartialOffDayTypeID",
                table: "EmployeeHours");

            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeHoursAwaitingApprovals_PartialOffDayTypes_PartialOffDayTypeID",
                table: "EmployeeHoursAwaitingApprovals");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PartialOffDayTypes",
                table: "PartialOffDayTypes");

            migrationBuilder.DeleteData(
                table: "PartialOffDayTypes",
                keyColumn: "PartialOffDayTypeID",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "PartialOffDayTypes",
                keyColumn: "PartialOffDayTypeID",
                keyValue: 2);

            migrationBuilder.DropColumn(
                name: "PartialOffDayTypeID",
                table: "PartialOffDayTypes");

            migrationBuilder.RenameTable(
                name: "PartialOffDayTypes",
                newName: "PartialOffDayType");

            migrationBuilder.AlterColumn<DateTime>(
                name: "PartialOffDayHours",
                table: "EmployeeHoursAwaitingApprovals",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(TimeSpan),
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "PartialOffDayHours",
                table: "EmployeeHours",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(TimeSpan),
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OffDayTypeID",
                table: "PartialOffDayType",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PartialOffDayType",
                table: "PartialOffDayType",
                column: "OffDayTypeID");

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeHours_PartialOffDayType_PartialOffDayTypeID",
                table: "EmployeeHours",
                column: "PartialOffDayTypeID",
                principalTable: "PartialOffDayType",
                principalColumn: "OffDayTypeID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeHoursAwaitingApprovals_PartialOffDayType_PartialOffDayTypeID",
                table: "EmployeeHoursAwaitingApprovals",
                column: "PartialOffDayTypeID",
                principalTable: "PartialOffDayType",
                principalColumn: "OffDayTypeID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
