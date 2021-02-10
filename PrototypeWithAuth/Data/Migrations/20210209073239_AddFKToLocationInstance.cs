using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class AddFKToLocationInstance : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LabPartID",
                table: "LocationInstances",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LocationRoomInstanceID",
                table: "LocationInstances",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_LocationInstances_LabPartID",
                table: "LocationInstances",
                column: "LabPartID");

            migrationBuilder.CreateIndex(
                name: "IX_LocationInstances_LocationRoomInstanceID",
                table: "LocationInstances",
                column: "LocationRoomInstanceID");

            migrationBuilder.AddForeignKey(
                name: "FK_LocationInstances_LabParts_LabPartID",
                table: "LocationInstances",
                column: "LabPartID",
                principalTable: "LabParts",
                principalColumn: "LabPartID",
                onDelete: ReferentialAction.Restrict);

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
                name: "FK_LocationInstances_LabParts_LabPartID",
                table: "LocationInstances");

            migrationBuilder.DropForeignKey(
                name: "FK_LocationInstances_LocationRoomInstances_LocationRoomInstanceID",
                table: "LocationInstances");

            migrationBuilder.DropIndex(
                name: "IX_LocationInstances_LabPartID",
                table: "LocationInstances");

            migrationBuilder.DropIndex(
                name: "IX_LocationInstances_LocationRoomInstanceID",
                table: "LocationInstances");

            migrationBuilder.DropColumn(
                name: "LabPartID",
                table: "LocationInstances");

            migrationBuilder.DropColumn(
                name: "LocationRoomInstanceID",
                table: "LocationInstances");
        }
    }
}
