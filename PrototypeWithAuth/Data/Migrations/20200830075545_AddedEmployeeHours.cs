using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class AddedEmployeeHours : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EmployeeHoursStatuses",
                columns: table => new
                {
                    EmployeeHoursStatusID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeHoursStatuses", x => x.EmployeeHoursStatusID);
                });

            migrationBuilder.CreateTable(
                name: "OffDayTypes",
                columns: table => new
                {
                    OffDayTypeID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OffDayTypes", x => x.OffDayTypeID);
                });

            migrationBuilder.CreateTable(
                name: "EmployeeHours",
                columns: table => new
                {
                    EmployeeHoursID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Entry1 = table.Column<DateTime>(nullable: false),
                    Entry2 = table.Column<DateTime>(nullable: false),
                    Exit1 = table.Column<DateTime>(nullable: false),
                    Exit2 = table.Column<DateTime>(nullable: false),
                    OffDayTypeID = table.Column<int>(nullable: true),
                    EmployeeHoursStatusID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeHours", x => x.EmployeeHoursID);
                    table.ForeignKey(
                        name: "FK_EmployeeHours_EmployeeHoursStatuses_EmployeeHoursStatusID",
                        column: x => x.EmployeeHoursStatusID,
                        principalTable: "EmployeeHoursStatuses",
                        principalColumn: "EmployeeHoursStatusID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EmployeeHours_OffDayTypes_OffDayTypeID",
                        column: x => x.OffDayTypeID,
                        principalTable: "OffDayTypes",
                        principalColumn: "OffDayTypeID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeHours_EmployeeHoursStatusID",
                table: "EmployeeHours",
                column: "EmployeeHoursStatusID");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeHours_OffDayTypeID",
                table: "EmployeeHours",
                column: "OffDayTypeID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmployeeHours");

            migrationBuilder.DropTable(
                name: "EmployeeHoursStatuses");

            migrationBuilder.DropTable(
                name: "OffDayTypes");
        }
    }
}
