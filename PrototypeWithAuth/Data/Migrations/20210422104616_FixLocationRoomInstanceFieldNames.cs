using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class FixLocationRoomInstanceFieldNames : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LocationRoomInstancName",
                table: "LocationRoomInstances");

            migrationBuilder.DropColumn(
                name: "LocationRoomInstanceAbbrv",
                table: "LocationRoomInstances");

            migrationBuilder.AddColumn<string>(
                name: "LocationRoomInstanceAbbrev",
                table: "LocationRoomInstances",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LocationRoomInstanceName",
                table: "LocationRoomInstances",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LocationRoomInstanceAbbrev",
                table: "LocationRoomInstances");

            migrationBuilder.DropColumn(
                name: "LocationRoomInstanceName",
                table: "LocationRoomInstances");

            migrationBuilder.AddColumn<string>(
                name: "LocationRoomInstancName",
                table: "LocationRoomInstances",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LocationRoomInstanceAbbrv",
                table: "LocationRoomInstances",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
