using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class addedLastCentarixIDTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LastCentarixIDs",
                columns: table => new
                {
                    LastCentarixIDID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeStatusID = table.Column<int>(nullable: false),
                    Number = table.Column<int>(nullable: false),
                    TimeStamp = table.Column<DateTime>(nullable: false)
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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LastCentarixIDs");
        }
    }
}
