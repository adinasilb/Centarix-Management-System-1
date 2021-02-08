using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class AddPhysicalAddressTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PhysicalAddresses",
                columns: table => new
                {
                    PhysicalAddressID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeID = table.Column<string>(nullable: true),
                    Address = table.Column<string>(nullable: true),
                    DateCreated = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PhysicalAddresses", x => x.PhysicalAddressID);
                    table.ForeignKey(
                        name: "FK_PhysicalAddresses_AspNetUsers_EmployeeID",
                        column: x => x.EmployeeID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PhysicalAddresses_EmployeeID",
                table: "PhysicalAddresses",
                column: "EmployeeID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PhysicalAddresses");
        }
    }
}
