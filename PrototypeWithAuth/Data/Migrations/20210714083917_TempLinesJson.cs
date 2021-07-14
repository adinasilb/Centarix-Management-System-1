using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class TempLinesJson : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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
                name: "IsTemporary",
                table: "Lines");

            migrationBuilder.DropColumn(
                name: "TempLineLineID",
                table: "Lines");

            migrationBuilder.DropColumn(
                name: "IsTemporary",
                table: "FunctionLines");

            migrationBuilder.CreateTable(
                name: "TempLinesJsons",
                columns: table => new
                {
                    TempLinesJsonID = table.Column<Guid>(nullable: false),
                    Json = table.Column<string>(type: "ntext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TempLinesJsons", x => x.TempLinesJsonID);
                });

            //migrationBuilder.CreateTable(
            //    name: "TempRequestJsons",
            //    columns: table => new
            //    {
            //        TempRequestJsonID = table.Column<int>(nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        GuidID = table.Column<Guid>(nullable: false),
            //        ApplicationUserID = table.Column<string>(nullable: true),
            //        RequestJson = table.Column<string>(nullable: true),
            //        IsOriginal = table.Column<bool>(nullable: false),
            //        IsCurrent = table.Column<bool>(nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_TempRequestJsons", x => x.TempRequestJsonID);
            //        table.ForeignKey(
            //            name: "FK_TempRequestJsons_AspNetUsers_ApplicationUserID",
            //            column: x => x.ApplicationUserID,
            //            principalTable: "AspNetUsers",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Restrict);
            //    });

            //migrationBuilder.CreateIndex(
            //    name: "IX_TempRequestJsons_ApplicationUserID",
            //    table: "TempRequestJsons",
            //    column: "ApplicationUserID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TempLinesJsons");

            migrationBuilder.DropTable(
                name: "TempRequestJsons");

            migrationBuilder.AddColumn<bool>(
                name: "IsTemporary",
                table: "Lines",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "TempLineLineID",
                table: "Lines",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsTemporary",
                table: "FunctionLines",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "TempLines",
                columns: table => new
                {
                    LineID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LineNumber = table.Column<int>(type: "int", nullable: false),
                    LineTypeID = table.Column<int>(type: "int", nullable: false),
                    ParentLineID = table.Column<int>(type: "int", nullable: true),
                    PermanentLineID = table.Column<int>(type: "int", nullable: true),
                    ProtocolID = table.Column<int>(type: "int", nullable: false),
                    UniqueGuid = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TempLines", x => x.LineID);
                    table.ForeignKey(
                        name: "FK_TempLines_LineTypes_LineTypeID",
                        column: x => x.LineTypeID,
                        principalTable: "LineTypes",
                        principalColumn: "LineTypeID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TempLines_TempLines_ParentLineID",
                        column: x => x.ParentLineID,
                        principalTable: "TempLines",
                        principalColumn: "LineID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TempLines_Lines_PermanentLineID",
                        column: x => x.PermanentLineID,
                        principalTable: "Lines",
                        principalColumn: "LineID",
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
                name: "IX_TempLines_ParentLineID",
                table: "TempLines",
                column: "ParentLineID");

            migrationBuilder.CreateIndex(
                name: "IX_TempLines_PermanentLineID",
                table: "TempLines",
                column: "PermanentLineID",
                unique: true);

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
    }
}
