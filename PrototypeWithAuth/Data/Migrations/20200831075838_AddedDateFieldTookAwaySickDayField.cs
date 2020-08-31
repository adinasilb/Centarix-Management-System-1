using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class AddedDateFieldTookAwaySickDayField : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SickDays",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<DateTime>(
                name: "Date",
                table: "EmployeeHours",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Date",
                table: "EmployeeHours");

            migrationBuilder.AddColumn<int>(
                name: "SickDays",
                table: "AspNetUsers",
                type: "int",
                nullable: true);
        }
    }
}
