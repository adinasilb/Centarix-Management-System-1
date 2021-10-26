using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class sequencePositionForTests : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SequencePosition",
                table: "TestOuterGroups",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SequencePosition",
                table: "TestHeaders",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SequencePosition",
                table: "TestGroups",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SequencePosition",
                table: "TestOuterGroups");

            migrationBuilder.DropColumn(
                name: "SequencePosition",
                table: "TestHeaders");

            migrationBuilder.DropColumn(
                name: "SequencePosition",
                table: "TestGroups");
        }
    }
}
