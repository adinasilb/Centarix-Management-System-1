using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class AddQuartzyFields2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "LocationInstances",
                columns: new[] { "LocationInstanceID", "ContainsItems", "Discriminator", "Height", "IsEmptyShelf", "IsFull", "LabPartID", "LocationInstanceAbbrev", "LocationInstanceName", "LocationInstanceParentID", "LocationNumber", "LocationRoomInstanceID", "LocationTypeID", "Width" },
                values: new object[] { -1, false, "TemporaryLocationInstance", 0, true, false, null, "Quartzy", "Quartzy Temporary", null, 1, null, 600, 0 });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "LocationInstances",
                keyColumn: "LocationInstanceID",
                keyValue: -1);
        }
    }
}
