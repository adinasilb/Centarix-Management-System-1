using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class StartedRedoLocationsAgain2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DeleteData(
            //    table: "LocationInstances",
            //    keyColumn: "LocationInstanceID",
            //    keyValue: 1);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
        //    migrationBuilder.InsertData(
        //        table: "LocationInstances",
        //        columns: new[] { "LocationInstanceID", "ContainsItems", "Height", "IsEmptyShelf", "IsFull", "LabPartID", "LocationInstanceAbbrev", "LocationInstanceName", "LocationInstanceParentID", "LocationNumber", "LocationRoomInstanceID", "LocationTypeID", "Width" },
        //        values: new object[] { 1, false, 7, false, false, null, null, "25°C", null, 0, null, 500, 1 });
        }
    }
}
