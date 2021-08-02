using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class DescriptionAndTimerToFunctionBase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "FunctionResults",
                nullable: true);

            migrationBuilder.AddColumn<TimeSpan>(
                name: "Timer",
                table: "FunctionResults",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "FunctionReports",
                nullable: true);

            migrationBuilder.AddColumn<TimeSpan>(
                name: "Timer",
                table: "FunctionReports",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "FunctionResults");

            migrationBuilder.DropColumn(
                name: "Timer",
                table: "FunctionResults");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "FunctionReports");

            migrationBuilder.DropColumn(
                name: "Timer",
                table: "FunctionReports");
        }
    }
}
