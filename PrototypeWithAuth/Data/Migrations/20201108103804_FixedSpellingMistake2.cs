using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class FixedSpellingMistake2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OperaitonOrderLimit",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<double>(
                name: "OperationOrderLimit",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OperationOrderLimit",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<double>(
                name: "OperaitonOrderLimit",
                table: "AspNetUsers",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}
