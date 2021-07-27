using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class TempLinesJson2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RequestJson",
                table: "TempRequestJsons");

            migrationBuilder.AddColumn<string>(
                name: "Json",
                table: "TempRequestJsons",
                type: "ntext",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Json",
                table: "TempRequestJsons");

            migrationBuilder.AddColumn<string>(
                name: "RequestJson",
                table: "TempRequestJsons",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
