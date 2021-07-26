using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class AddedIsTempDeletedField : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
              

            migrationBuilder.AddColumn<bool>(
                name: "IsTemporaryDeleted",
                table: "Lines",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.DropColumn(
                name: "IsTemporaryDeleted",
                table: "Lines");


        }
    }
}
