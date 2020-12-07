using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class changedLocationInstanceBooleans : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsEmpty",
                table: "LocationInstances");

            migrationBuilder.AddColumn<bool>(
                name: "ContainsItems",
                table: "LocationInstances",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsEmptyShelf",
                table: "LocationInstances",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ContainsItems",
                table: "LocationInstances");

            migrationBuilder.DropColumn(
                name: "IsEmptyShelf",
                table: "LocationInstances");

            migrationBuilder.AddColumn<bool>(
                name: "IsEmpty",
                table: "LocationInstances",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
