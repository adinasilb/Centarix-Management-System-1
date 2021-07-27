using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class AddedLineChangeTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LineChanges",
                columns: table => new
                {
                    LineID = table.Column<int>(nullable: false),
                    ProtocolInstanceID = table.Column<int>(nullable: false),
                    ChangeText = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LineChanges", x => new { x.LineID, x.ProtocolInstanceID });
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LineChanges");
        }
    }
}
