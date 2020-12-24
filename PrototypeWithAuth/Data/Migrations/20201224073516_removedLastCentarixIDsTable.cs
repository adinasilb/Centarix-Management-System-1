using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class removedLastCentarixIDsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LastCentarixIDs");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LastCentarixIDs",
                columns: table => new
                {
                    LastCentarixIDID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Abbreviation = table.Column<string>(type: "char(2)", fixedLength: true, maxLength: 2, nullable: true),
                    EmployeeStatusID = table.Column<int>(type: "int", nullable: false),
                    Number = table.Column<int>(type: "int", nullable: false),
                    TimeStamp = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LastCentarixIDs", x => x.LastCentarixIDID);
                    table.ForeignKey(
                        name: "FK_LastCentarixIDs_EmployeeStatuses_EmployeeStatusID",
                        column: x => x.EmployeeStatusID,
                        principalTable: "EmployeeStatuses",
                        principalColumn: "EmployeeStatusID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LastCentarixIDs_EmployeeStatusID",
                table: "LastCentarixIDs",
                column: "EmployeeStatusID");
        }
    }
}
