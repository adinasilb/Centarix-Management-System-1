using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class LocationInstanceRequestFKAndPlaceProp : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Place",
                table: "LocationInstances",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RequestID",
                table: "LocationInstances",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_LocationInstances_RequestID",
                table: "LocationInstances",
                column: "RequestID");

            migrationBuilder.AddForeignKey(
                name: "FK_LocationInstances_Requests_RequestID",
                table: "LocationInstances",
                column: "RequestID",
                principalTable: "Requests",
                principalColumn: "RequestID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LocationInstances_Requests_RequestID",
                table: "LocationInstances");

            migrationBuilder.DropIndex(
                name: "IX_LocationInstances_RequestID",
                table: "LocationInstances");

            migrationBuilder.DropColumn(
                name: "Place",
                table: "LocationInstances");

            migrationBuilder.DropColumn(
                name: "RequestID",
                table: "LocationInstances");
        }
    }
}
