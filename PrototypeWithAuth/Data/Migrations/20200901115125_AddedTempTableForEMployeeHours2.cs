using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class AddedTempTableForEMployeeHours2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EmployeeHoursAwaitingApprovals",
                columns: table => new
                {
                    EmployeeHoursID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeID = table.Column<string>(nullable: true),
                    Entry1 = table.Column<DateTime>(nullable: false),
                    Entry2 = table.Column<DateTime>(nullable: true),
                    Exit1 = table.Column<DateTime>(nullable: true),
                    Exit2 = table.Column<DateTime>(nullable: true),
                    Date = table.Column<DateTime>(nullable: false),
                    OffDayTypeID = table.Column<int>(nullable: true),
                    EmployeeHoursStatusID = table.Column<int>(nullable: true),
                    TotalHours = table.Column<TimeSpan>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeHoursAwaitingApprovals", x => x.EmployeeHoursID);
                    table.ForeignKey(
                        name: "FK_EmployeeHoursAwaitingApprovals_EmployeeHoursStatuses_EmployeeHoursStatusID",
                        column: x => x.EmployeeHoursStatusID,
                        principalTable: "EmployeeHoursStatuses",
                        principalColumn: "EmployeeHoursStatusID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EmployeeHoursAwaitingApprovals_AspNetUsers_EmployeeID",
                        column: x => x.EmployeeID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EmployeeHoursAwaitingApprovals_OffDayTypes_OffDayTypeID",
                        column: x => x.OffDayTypeID,
                        principalTable: "OffDayTypes",
                        principalColumn: "OffDayTypeID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "OffDayTypes",
                columns: new[] { "OffDayTypeID", "Description" },
                values: new object[] { 1, "SickDay" });

            migrationBuilder.InsertData(
                table: "OffDayTypes",
                columns: new[] { "OffDayTypeID", "Description" },
                values: new object[] { 2, " VacationDay" });

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeHoursAwaitingApprovals_EmployeeHoursStatusID",
                table: "EmployeeHoursAwaitingApprovals",
                column: "EmployeeHoursStatusID");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeHoursAwaitingApprovals_EmployeeID",
                table: "EmployeeHoursAwaitingApprovals",
                column: "EmployeeID");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeHoursAwaitingApprovals_OffDayTypeID",
                table: "EmployeeHoursAwaitingApprovals",
                column: "OffDayTypeID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmployeeHoursAwaitingApprovals");

            migrationBuilder.DeleteData(
                table: "OffDayTypes",
                keyColumn: "OffDayTypeID",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "OffDayTypes",
                keyColumn: "OffDayTypeID",
                keyValue: 2);
        }
    }
}
