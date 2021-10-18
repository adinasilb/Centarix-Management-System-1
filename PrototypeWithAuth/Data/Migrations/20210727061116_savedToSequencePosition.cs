using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class savedToSequencePosition : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SequenceSaved",
                table: "TempRequestJsons");

            migrationBuilder.AddColumn<int>(
                name: "SequencePosition",
                table: "TempRequestJsons",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SequencePosition",
                table: "TempRequestJsons");

            migrationBuilder.AddColumn<int>(
                name: "SequenceSaved",
                table: "TempRequestJsons",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
