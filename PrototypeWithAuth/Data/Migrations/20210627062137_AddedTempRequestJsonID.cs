using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class AddedTempRequestJsonID : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_TempRequestJsons",
                table: "TempRequestJsons");

            migrationBuilder.AddColumn<int>(
                name: "TempRequestJsonID",
                table: "TempRequestJsons",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TempRequestJsons",
                table: "TempRequestJsons",
                column: "TempRequestJsonID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_TempRequestJsons",
                table: "TempRequestJsons");

            migrationBuilder.DropColumn(
                name: "TempRequestJsonID",
                table: "TempRequestJsons");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TempRequestJsons",
                table: "TempRequestJsons",
                column: "GuidID");
        }
    }
}
