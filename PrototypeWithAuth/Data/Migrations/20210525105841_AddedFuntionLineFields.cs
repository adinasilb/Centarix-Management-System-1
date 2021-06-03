using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class AddedFuntionLineFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_EmployeeHoursAwaitingApprovals_EmployeeHoursID",
                table: "EmployeeHoursAwaitingApprovals");

            migrationBuilder.DropColumn(
                name: "Timer",
                table: "TempLines");

            migrationBuilder.DropColumn(
                name: "Timer",
                table: "Lines");

            migrationBuilder.AddColumn<int>(
                name: "FunctionLineID",
                table: "FunctionLines",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ProductID",
                table: "FunctionLines",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ProtocolID",
                table: "FunctionLines",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Text",
                table: "FunctionLines",
                nullable: true);

            migrationBuilder.AddColumn<TimeSpan>(
                name: "Timer",
                table: "FunctionLines",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));

            migrationBuilder.CreateIndex(
                name: "IX_FunctionLines_ProductID",
                table: "FunctionLines",
                column: "ProductID");

            migrationBuilder.CreateIndex(
                name: "IX_FunctionLines_ProtocolID",
                table: "FunctionLines",
                column: "ProtocolID");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeHoursAwaitingApprovals_EmployeeHoursID",
                table: "EmployeeHoursAwaitingApprovals",
                column: "EmployeeHoursID",
                unique: true,
                filter: "[EmployeeHoursID] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_FunctionLines_Products_ProductID",
                table: "FunctionLines",
                column: "ProductID",
                principalTable: "Products",
                principalColumn: "ProductID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FunctionLines_Protocols_ProtocolID",
                table: "FunctionLines",
                column: "ProtocolID",
                principalTable: "Protocols",
                principalColumn: "ProtocolID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FunctionLines_Products_ProductID",
                table: "FunctionLines");

            migrationBuilder.DropForeignKey(
                name: "FK_FunctionLines_Protocols_ProtocolID",
                table: "FunctionLines");

            migrationBuilder.DropIndex(
                name: "IX_FunctionLines_ProductID",
                table: "FunctionLines");

            migrationBuilder.DropIndex(
                name: "IX_FunctionLines_ProtocolID",
                table: "FunctionLines");

            migrationBuilder.DropIndex(
                name: "IX_EmployeeHoursAwaitingApprovals_EmployeeHoursID",
                table: "EmployeeHoursAwaitingApprovals");

            migrationBuilder.DropColumn(
                name: "FunctionLineID",
                table: "FunctionLines");

            migrationBuilder.DropColumn(
                name: "ProductID",
                table: "FunctionLines");

            migrationBuilder.DropColumn(
                name: "ProtocolID",
                table: "FunctionLines");

            migrationBuilder.DropColumn(
                name: "Text",
                table: "FunctionLines");

            migrationBuilder.DropColumn(
                name: "Timer",
                table: "FunctionLines");

            migrationBuilder.AddColumn<TimeSpan>(
                name: "Timer",
                table: "TempLines",
                type: "time",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));

            migrationBuilder.AddColumn<TimeSpan>(
                name: "Timer",
                table: "Lines",
                type: "time",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeHoursAwaitingApprovals_EmployeeHoursID",
                table: "EmployeeHoursAwaitingApprovals",
                column: "EmployeeHoursID");
        }
    }
}
