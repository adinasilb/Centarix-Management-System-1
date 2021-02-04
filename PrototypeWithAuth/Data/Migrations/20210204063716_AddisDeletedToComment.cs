using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class AddisDeletedToComment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "isDeleted",
                table: "Payments",
                newName: "IsDeleted");

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Comments",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Comments");

            migrationBuilder.RenameColumn(
                name: "IsDeleted",
                table: "Payments",
                newName: "isDeleted");
        }
    }
}
