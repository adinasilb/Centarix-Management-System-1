using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class RenameRecurringOrderOccurrences : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Occurances",
                table: "Products");

            migrationBuilder.AddColumn<int>(
                name: "Occurrences",
                table: "Products",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Occurrences",
                table: "Products");

            migrationBuilder.AddColumn<int>(
                name: "Occurances",
                table: "Products",
                type: "int",
                nullable: true);
        }
    }
}
