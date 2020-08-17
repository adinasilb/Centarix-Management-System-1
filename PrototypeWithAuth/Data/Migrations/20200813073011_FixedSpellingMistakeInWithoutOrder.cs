using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class FixedSpellingMistakeInWithoutOrder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "WithOutOrder",
                table: "ParentRequests",
                newName: "WithoutOrder");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "WithoutOrder",
                table: "ParentRequests",
                newName: "WithOutOrder");
        }
    }
}
