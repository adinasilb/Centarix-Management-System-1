using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class AddedLocationRoomInstanceNAme : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LocationRoomInstanceName",
                table: "LocationRoomInstances",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "LocationRoomInstances",
                keyColumn: "LocationRoomInstanceID",
                keyValue: 1,
                column: "LocationRoomInstanceName",
                value: "Laboratory 1");

            migrationBuilder.UpdateData(
                table: "LocationRoomInstances",
                keyColumn: "LocationRoomInstanceID",
                keyValue: 2,
                column: "LocationRoomInstanceName",
                value: "Laboratory 2");

            migrationBuilder.UpdateData(
                table: "LocationRoomInstances",
                keyColumn: "LocationRoomInstanceID",
                keyValue: 3,
                column: "LocationRoomInstanceName",
                value: "Tissue Culture 1");

            migrationBuilder.UpdateData(
                table: "LocationRoomInstances",
                keyColumn: "LocationRoomInstanceID",
                keyValue: 4,
                column: "LocationRoomInstanceName",
                value: "Equipment Room 1");

            migrationBuilder.UpdateData(
                table: "LocationRoomInstances",
                keyColumn: "LocationRoomInstanceID",
                keyValue: 5,
                column: "LocationRoomInstanceName",
                value: "Refrigerator Room 1");

            migrationBuilder.UpdateData(
                table: "LocationRoomInstances",
                keyColumn: "LocationRoomInstanceID",
                keyValue: 6,
                column: "LocationRoomInstanceName",
                value: "Washing Room 1");

            migrationBuilder.UpdateData(
                table: "LocationRoomInstances",
                keyColumn: "LocationRoomInstanceID",
                keyValue: 7,
                column: "LocationRoomInstanceName",
                value: "Storage Room 1");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LocationRoomInstanceName",
                table: "LocationRoomInstances");
        }
    }
}
