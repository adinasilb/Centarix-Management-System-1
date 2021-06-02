using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class LocationInstanceForeignKeyRoomInstance : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LocationRoomInstanceID",
                table: "LocationInstances",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_LocationInstances_LocationRoomInstanceID",
                table: "LocationInstances",
                column: "LocationRoomInstanceID");

            migrationBuilder.AddForeignKey(
                name: "FK_LocationInstances_LocationRoomInstances_LocationRoomInstanceID",
                table: "LocationInstances",
                column: "LocationRoomInstanceID",
                principalTable: "LocationRoomInstances",
                principalColumn: "LocationRoomInstanceID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LocationInstances_LocationRoomInstances_LocationRoomInstanceID",
                table: "LocationInstances");

            migrationBuilder.DropIndex(
                name: "IX_LocationInstances_LocationRoomInstanceID",
                table: "LocationInstances");

            migrationBuilder.DropColumn(
                name: "LocationRoomInstanceID",
                table: "LocationInstances");
        }
    }
}
