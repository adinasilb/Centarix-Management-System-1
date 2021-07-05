using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class ArchiveRequestLocationInstance : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "RequestLocationInstances");

            migrationBuilder.AddColumn<bool>(
                name: "IsArchived",
                table: "RequestLocationInstances",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsArchived",
                table: "RequestLocationInstances");

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "RequestLocationInstances",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
