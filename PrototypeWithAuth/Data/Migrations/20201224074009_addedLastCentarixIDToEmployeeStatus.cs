using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class addedLastCentarixIDToEmployeeStatus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Abbreviation",
                table: "EmployeeStatuses",
                type: "char(2)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LastCentarixID",
                table: "EmployeeStatuses",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastCentarixIDTimeStamp",
                table: "EmployeeStatuses",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Abbreviation",
                table: "EmployeeStatuses");

            migrationBuilder.DropColumn(
                name: "LastCentarixID",
                table: "EmployeeStatuses");

            migrationBuilder.DropColumn(
                name: "LastCentarixIDTimeStamp",
                table: "EmployeeStatuses");
        }
    }
}
