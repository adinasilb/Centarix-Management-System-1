using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class FillLocationRoomInstances : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "LocationRoomInstances",
                columns: new[] { "LocationRoomInstanceID", "LocationRoomInstanceAbbrev", "LocationRoomInstanceName", "LocationRoomTypeID" },
                values: new object[,]
                {
                    { 1, "L1", "Laboratory 1", 1 },
                    { 2, "L2", "Laboratory 2", 1 },
                    { 3, "TC1", "Tissue Culture 1", 2 },
                    { 4, "E1", "Equipment Room 1", 3 },
                    { 5, "R1", "Refrigerator Room 1", 4 },
                    { 6, "W1", "Washing Room 1", 5 },
                    { 7, "S1", "Storage Room 1", 6 }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "LocationRoomInstances",
                keyColumn: "LocationRoomInstanceID",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "LocationRoomInstances",
                keyColumn: "LocationRoomInstanceID",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "LocationRoomInstances",
                keyColumn: "LocationRoomInstanceID",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "LocationRoomInstances",
                keyColumn: "LocationRoomInstanceID",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "LocationRoomInstances",
                keyColumn: "LocationRoomInstanceID",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "LocationRoomInstances",
                keyColumn: "LocationRoomInstanceID",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "LocationRoomInstances",
                keyColumn: "LocationRoomInstanceID",
                keyValue: 7);
        }
    }
}
