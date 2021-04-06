using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class AddLocationMigrationData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "HasShelves",
                table: "LabParts",
                nullable: false,
                defaultValue: false);

            migrationBuilder.InsertData(
                table: "LabParts",
                columns: new[] { "LabPartID", "HasShelves", "LabPartName" },
                values: new object[,]
                {
                    { 1, true, "Closet" },
                    { 2, true, "Glass Closet" },
                    { 3, false, "Table" },
                    { 4, false, "Drawer" },
                    { 5, false, "Shelf" }
                });

            migrationBuilder.InsertData(
                table: "LocationRoomInstances",
                columns: new[] { "LocationRoomInstanceID", "LocationNumber", "LocationRoomTypeID" },
                values: new object[,]
                {
                    { 1, 1, 1 },
                    { 2, 2, 1 },
                    { 3, 1, 2 },
                    { 4, 1, 3 },
                    { 5, 1, 4 },
                    { 6, 1, 5 },
                    { 7, 1, 6 }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "LabParts",
                keyColumn: "LabPartID",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "LabParts",
                keyColumn: "LabPartID",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "LabParts",
                keyColumn: "LabPartID",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "LabParts",
                keyColumn: "LabPartID",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "LabParts",
                keyColumn: "LabPartID",
                keyValue: 5);

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

            migrationBuilder.DropColumn(
                name: "HasShelves",
                table: "LabParts");
        }
    }
}
