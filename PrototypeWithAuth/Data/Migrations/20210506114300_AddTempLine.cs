using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class AddTempLine : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Lines_Lines_ParentLineID",
                table: "Lines");

            migrationBuilder.DropIndex(
                name: "IX_Lines_ParentLineID",
                table: "Lines");

            migrationBuilder.DropColumn(
                name: "ParentLineID",
                table: "Lines");

            migrationBuilder.AddColumn<int>(
                name: "TempLineLineID",
                table: "Lines",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "TempLines",
                columns: table => new
                {
                    LineID = table.Column<int>(nullable: false),
                    Content = table.Column<string>(nullable: true),
                    LineTypeID = table.Column<int>(nullable: false),
                    ProtocolID = table.Column<int>(nullable: false),
                    LineNumber = table.Column<int>(nullable: false),
                    Timer = table.Column<TimeSpan>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TempLines", x => x.LineID);
                    table.ForeignKey(
                        name: "FK_TempLines_Lines_LineID",
                        column: x => x.LineID,
                        principalTable: "Lines",
                        principalColumn: "LineID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TempLines_LineTypes_LineTypeID",
                        column: x => x.LineTypeID,
                        principalTable: "LineTypes",
                        principalColumn: "LineTypeID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TempLines_Protocols_ProtocolID",
                        column: x => x.ProtocolID,
                        principalTable: "Protocols",
                        principalColumn: "ProtocolID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Lines_TempLineLineID",
                table: "Lines",
                column: "TempLineLineID");

            migrationBuilder.CreateIndex(
                name: "IX_TempLines_LineTypeID",
                table: "TempLines",
                column: "LineTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_TempLines_ProtocolID",
                table: "TempLines",
                column: "ProtocolID");

            migrationBuilder.AddForeignKey(
                name: "FK_Lines_TempLines_TempLineLineID",
                table: "Lines",
                column: "TempLineLineID",
                principalTable: "TempLines",
                principalColumn: "LineID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Lines_TempLines_TempLineLineID",
                table: "Lines");

            migrationBuilder.DropTable(
                name: "TempLines");

            migrationBuilder.DropIndex(
                name: "IX_Lines_TempLineLineID",
                table: "Lines");

            migrationBuilder.DropColumn(
                name: "TempLineLineID",
                table: "Lines");

            migrationBuilder.AddColumn<int>(
                name: "ParentLineID",
                table: "Lines",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Lines_ParentLineID",
                table: "Lines",
                column: "ParentLineID");

            migrationBuilder.AddForeignKey(
                name: "FK_Lines_Lines_ParentLineID",
                table: "Lines",
                column: "ParentLineID",
                principalTable: "Lines",
                principalColumn: "LineID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
