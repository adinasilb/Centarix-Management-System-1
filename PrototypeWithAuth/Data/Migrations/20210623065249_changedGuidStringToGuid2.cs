using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class changedGuidStringToGuid2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_TempRequestJsons",
                table: "TempRequestJsons");

            migrationBuilder.DropColumn(
                name: "TempID",
                table: "TempRequestJsons");

            migrationBuilder.DropColumn(
                name: "CookieGUID",
                table: "TempRequestJsons");

            migrationBuilder.AddColumn<Guid>(
                name: "GuidID",
                table: "TempRequestJsons",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_TempRequestJsons",
                table: "TempRequestJsons",
                column: "GuidID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_TempRequestJsons",
                table: "TempRequestJsons");

            migrationBuilder.DropColumn(
                name: "GuidID",
                table: "TempRequestJsons");

            migrationBuilder.AddColumn<int>(
                name: "TempID",
                table: "TempRequestJsons",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<Guid>(
                name: "CookieGUID",
                table: "TempRequestJsons",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_TempRequestJsons",
                table: "TempRequestJsons",
                column: "TempID");
        }
    }
}
