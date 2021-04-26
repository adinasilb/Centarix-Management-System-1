using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class RemoveChildLocationInstances : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "LocationInstances",
                keyColumn: "LocationInstanceID",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "LocationInstances",
                keyColumn: "LocationInstanceID",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "LocationInstances",
                keyColumn: "LocationInstanceID",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "LocationInstances",
                keyColumn: "LocationInstanceID",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "LocationInstances",
                keyColumn: "LocationInstanceID",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "LocationInstances",
                keyColumn: "LocationInstanceID",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "LocationInstances",
                keyColumn: "LocationInstanceID",
                keyValue: 8);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "LocationInstances",
                columns: new[] { "LocationInstanceID", "CompanyLocationNo", "ContainsItems", "Height", "IsEmptyShelf", "IsFull", "LabPartID", "LocationInstanceAbbrev", "LocationInstanceName", "LocationInstanceParentID", "LocationNumber", "LocationRoomInstanceID", "LocationTypeID", "Place", "Width" },
                values: new object[,]
                {
                    { 2, 0, false, 0, false, false, null, "L1", "Laboratory 1", 1, 0, null, 501, null, 1 },
                    { 3, 0, false, 0, false, false, null, "L2", "Laboratory 2", 1, 0, null, 501, null, 1 },
                    { 4, 0, false, 0, false, false, null, "TC1", "Tissue Culture 1", 1, 0, null, 501, null, 1 },
                    { 5, 0, false, 0, false, false, null, "E1", "Equipment Room 1", 1, 0, null, 501, null, 1 },
                    { 6, 0, false, 0, false, false, null, "R1", "Refrigerator Room 1", 1, 0, null, 501, null, 1 },
                    { 7, 0, false, 0, false, false, null, "W1", "Washing Room 1", 1, 0, null, 501, null, 1 },
                    { 8, 0, false, 0, false, false, null, "S1", "Storage Room 1", 1, 0, null, 501, null, 1 }
                });
        }
    }
}
