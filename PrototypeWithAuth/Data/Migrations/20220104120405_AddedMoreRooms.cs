using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class AddedMoreRooms : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "LocationRoomInstances",
                columns: new[] { "LocationRoomInstanceID", "LocationRoomInstanceAbbrev", "LocationRoomInstanceName", "LocationRoomTypeID" },
                values: new object[] { 14, "S2", "Storage Room 2", 6 });

            migrationBuilder.InsertData(
                table: "LocationRoomInstances",
                columns: new[] { "LocationRoomInstanceID", "LocationRoomInstanceAbbrev", "LocationRoomInstanceName", "LocationRoomTypeID" },
                values: new object[] { 15, "S3", "Storage Room 3", 6 });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "LocationRoomInstances",
                keyColumn: "LocationRoomInstanceID",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "LocationRoomInstances",
                keyColumn: "LocationRoomInstanceID",
                keyValue: 15);
        }
    }
}
