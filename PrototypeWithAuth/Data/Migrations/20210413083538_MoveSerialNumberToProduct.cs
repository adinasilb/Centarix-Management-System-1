using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class MoveSerialNumberToProduct : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SerialNumber",
                table: "Requests");

            migrationBuilder.AddColumn<string>(
                name: "SerialNumber",
                table: "Products",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SerialNumber",
                table: "Products");

            migrationBuilder.AddColumn<string>(
                name: "SerialNumber",
                table: "Requests",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
