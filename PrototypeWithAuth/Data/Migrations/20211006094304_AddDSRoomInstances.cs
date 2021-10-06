using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class AddDSRoomInstances : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "LocationRoomInstances",
                columns: new[] { "LocationRoomInstanceID", "LocationRoomInstanceAbbrev", "LocationRoomInstanceName", "LocationRoomTypeID" },
                values: new object[,]
                {
                    //commented out rooms were previously seeded
                    //{ 1, "L1", "Laboratory 1", 1 },
                    //{ 2, "L2", "Laboratory 2", 1 },
                    //{ 3, "TC1", "Tissue Culture 1", 2 },
                    //{ 4, "E1", "Equipment Room 1", 3 },
                    //{ 5, "R1", "Refrigerator Room 1", 4 },
                    //{ 6, "W1", "Washing Room 1", 5 },
                    //{ 7, "S1", "Storage Room 1", 6 },
                    { 8, "DSL3", "DS-Lab 3", 1 },
                    { 9, "DSL4", "DS-Lab 4", 1 },
                    { 10, "DSTC2", "DS-Tissue Culture 2", 2 },
                    { 11, "DSW2", "DS-Washing Room 2", 5 },
                    { 12, "LN1", "Liquid Nitrogen Room 1", 7 }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DeleteData(
            //    table: "LocationRoomInstances",
            //    keyColumn: "LocationRoomInstanceID",
            //    keyValue: 1);

            //migrationBuilder.DeleteData(
            //    table: "LocationRoomInstances",
            //    keyColumn: "LocationRoomInstanceID",
            //    keyValue: 2);

            //migrationBuilder.DeleteData(
            //    table: "LocationRoomInstances",
            //    keyColumn: "LocationRoomInstanceID",
            //    keyValue: 3);

            //migrationBuilder.DeleteData(
            //    table: "LocationRoomInstances",
            //    keyColumn: "LocationRoomInstanceID",
            //    keyValue: 4);

            //migrationBuilder.DeleteData(
            //    table: "LocationRoomInstances",
            //    keyColumn: "LocationRoomInstanceID",
            //    keyValue: 5);

            //migrationBuilder.DeleteData(
            //    table: "LocationRoomInstances",
            //    keyColumn: "LocationRoomInstanceID",
            //    keyValue: 6);

            //migrationBuilder.DeleteData(
            //    table: "LocationRoomInstances",
            //    keyColumn: "LocationRoomInstanceID",
            //    keyValue: 7);

            migrationBuilder.DeleteData(
                table: "LocationRoomInstances",
                keyColumn: "LocationRoomInstanceID",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "LocationRoomInstances",
                keyColumn: "LocationRoomInstanceID",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "LocationRoomInstances",
                keyColumn: "LocationRoomInstanceID",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "LocationRoomInstances",
                keyColumn: "LocationRoomInstanceID",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "LocationRoomInstances",
                keyColumn: "LocationRoomInstanceID",
                keyValue: 12);
        }
    }
}
