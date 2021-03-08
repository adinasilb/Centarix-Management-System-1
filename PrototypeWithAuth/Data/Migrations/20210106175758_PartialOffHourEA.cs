using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class PartialOffHourEA : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "PartialOffDayHours",
                table: "EmployeeHoursAwaitingApprovals",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "PartialOffDayTypeID",
                table: "EmployeeHoursAwaitingApprovals",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeHoursAwaitingApprovals_PartialOffDayTypeID",
                table: "EmployeeHoursAwaitingApprovals",
                column: "PartialOffDayTypeID");

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeHoursAwaitingApprovals_PartialOffDayType_PartialOffDayTypeID",
                table: "EmployeeHoursAwaitingApprovals",
                column: "PartialOffDayTypeID",
                principalTable: "PartialOffDayType",
                principalColumn: "OffDayTypeID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeHoursAwaitingApprovals_PartialOffDayType_PartialOffDayTypeID",
                table: "EmployeeHoursAwaitingApprovals");

            migrationBuilder.DropIndex(
                name: "IX_EmployeeHoursAwaitingApprovals_PartialOffDayTypeID",
                table: "EmployeeHoursAwaitingApprovals");

            migrationBuilder.DropColumn(
                name: "PartialOffDayHours",
                table: "EmployeeHoursAwaitingApprovals");

            migrationBuilder.DropColumn(
                name: "PartialOffDayTypeID",
                table: "EmployeeHoursAwaitingApprovals");
        }
    }
}
