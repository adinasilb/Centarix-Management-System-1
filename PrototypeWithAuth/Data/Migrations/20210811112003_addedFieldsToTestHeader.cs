using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class addedFieldsToTestHeader : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Calculation",
                table: "TestHeaders",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsSkip",
                table: "TestHeaders",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Calculation",
                table: "TestHeaders");

            migrationBuilder.DropColumn(
                name: "IsSkip",
                table: "TestHeaders");
        }
    }
}
