using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class TookAwayNoteForPartial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NoteForPartialDelivery",
                table: "Requests");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "NoteForPartialDelivery",
                table: "Requests",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
