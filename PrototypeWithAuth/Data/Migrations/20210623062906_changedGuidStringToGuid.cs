using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class changedGuidStringToGuid : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_TempRequestJsons",
                table: "TempRequestJsons");

            migrationBuilder.AlterColumn<Guid>(
                name: "CookieGUID",
                table: "TempRequestJsons",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<int>(
                name: "TempID",
                table: "TempRequestJsons",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<bool>(
                name: "IsCurrent",
                table: "TempRequestJsons",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsOriginal",
                table: "TempRequestJsons",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddPrimaryKey(
                name: "PK_TempRequestJsons",
                table: "TempRequestJsons",
                column: "TempID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_TempRequestJsons",
                table: "TempRequestJsons");

            migrationBuilder.DropColumn(
                name: "TempID",
                table: "TempRequestJsons");

            migrationBuilder.DropColumn(
                name: "IsCurrent",
                table: "TempRequestJsons");

            migrationBuilder.DropColumn(
                name: "IsOriginal",
                table: "TempRequestJsons");

            migrationBuilder.AlterColumn<string>(
                name: "CookieGUID",
                table: "TempRequestJsons",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(Guid));

            migrationBuilder.AddPrimaryKey(
                name: "PK_TempRequestJsons",
                table: "TempRequestJsons",
                column: "CookieGUID");
        }
    }
}
