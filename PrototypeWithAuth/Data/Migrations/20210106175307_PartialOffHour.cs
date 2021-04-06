using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class PartialOffHour : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "PartialOffDayHours",
                table: "EmployeeHours",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "PartialOffDayTypeID",
                table: "EmployeeHours",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "PartialOffDayType",
                columns: table => new
                {
                    OffDayTypeID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PartialOffDayType", x => x.OffDayTypeID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeHours_PartialOffDayTypeID",
                table: "EmployeeHours",
                column: "PartialOffDayTypeID");

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeHours_PartialOffDayType_PartialOffDayTypeID",
                table: "EmployeeHours",
                column: "PartialOffDayTypeID",
                principalTable: "PartialOffDayType",
                principalColumn: "OffDayTypeID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeHours_PartialOffDayType_PartialOffDayTypeID",
                table: "EmployeeHours");

            migrationBuilder.DropTable(
                name: "PartialOffDayType");

            migrationBuilder.DropIndex(
                name: "IX_EmployeeHours_PartialOffDayTypeID",
                table: "EmployeeHours");

            migrationBuilder.DropColumn(
                name: "PartialOffDayHours",
                table: "EmployeeHours");

            migrationBuilder.DropColumn(
                name: "PartialOffDayTypeID",
                table: "EmployeeHours");
        }
    }
}
